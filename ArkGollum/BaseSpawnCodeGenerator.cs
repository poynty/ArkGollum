using System.Text;
using System.Text.RegularExpressions;

namespace ArkGollum
{
    public interface ISpawnCodeGenerator
    {
        public string ProduceOutput(string[] files, Options options);
    }

    public abstract class BaseSpawnCodeGenerator
    {
        protected List<string> _sPlusItems = new List<string>();
        protected List<string> _simpleSpawnerItems = new List<string>();

        protected readonly string bannerEngramNames = "\n---------------------------------------------------------------------------------Engram Names------------------------------------------------------------------------------------\n";
        protected readonly string bannerItemSpawncodes = "\n---------------------------------------------------------------------------------Item Spawncodes---------------------------------------------------------------------------------\n";
        protected readonly string bannerCreatureSpawnCodes = "\n---------------------------------------------------------------------------------Creature Spawncodes-----------------------------------------------------------------------------\n";
        protected readonly string bannerTamedCreatureSpawnCodes = "\n---------------------------------------------------------------------------------Tamed Creature Spawncodes-----------------------------------------------------------------------\n";
        protected readonly string bannerSPlus = "\n---------------------------------------------------------------------------------S+ Pull Resources-------------------------------------------------------------------------------\n";
        protected readonly string bannerSimpleSpawners = "\n---------------------------------------------------------------------------------Simple Spawners---------------------------------------------------------------------------------\n";

        public virtual string ProduceOutput(string[] files, Options options)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(GetEngramNames(files));
            output.AppendLine(GetItemSpawnCodes(files, options));
            output.AppendLine(GetCreatureSpawnCodes(files));
            output.AppendLine(GetTamedCreatureSpawnCodes(files));
            if (options.SPlus)
            {
                output.AppendLine(GetSPlus(files));
            }
            if (options.SimpleSpawners)
            {
                output.AppendLine(GetSimpleSpawners(files));
            }
            return output.ToString();
        }

        protected virtual string GetEngramNames(string[] files)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerEngramNames);

            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.StartsWith("EngramEntry"))
                {
                    string str6 = withoutExtension + "_C";

                    sb.Append(str6 + Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        protected virtual string GetItemSpawnCodes(string[] files, Options options)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerItemSpawncodes);

            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.StartsWith("PrimalItem"))
                {
                    string str7 = "admincheat GiveItem \"Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 1 1 0";

                    sb.Append(str7 + Environment.NewLine);

                    if (IsRelevantPrimalItem(withoutExtension) && options.SPlus)
                    {
                        string itemText = GetPrimalItemLine(str7);
                        _sPlusItems.Add(itemText);
                    }
                }
            }

            return sb.ToString();
        }

        protected virtual string GetCreatureSpawnCodes(string[] files)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerCreatureSpawnCodes);

            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.Contains("Character_BP"))
                {
                    string str8 = "admincheat SpawnDino \"Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 500 0 0 120";
                    _simpleSpawnerItems.Add("Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'");
                    sb.Append(str8 + Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        protected virtual string GetTamedCreatureSpawnCodes(string[] files)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerTamedCreatureSpawnCodes);

            foreach (string path in files)
            {
                string str9 = Path.GetFileNameWithoutExtension(path) + "_C";
                if (str9.Contains("Character_BP"))
                {
                    string str10 = "admincheat GMSummon \"" + str9 + "\" 120";

                    sb.Append(str10 + Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        protected virtual string GetSPlus(string[] files)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerSPlus + Environment.NewLine);
            //Generate s+ pull list
            string sPlus = "PullResourceAdditions=";
            foreach (var resource in _sPlusItems)
            {
                sPlus = sPlus + resource + ",";
            }
            sPlus = sPlus.TrimEnd(',');
            sb.Append(sPlus + Environment.NewLine);
            return sb.ToString();
        }

        protected virtual string GetSimpleSpawners(string[] files)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerSimpleSpawners + Environment.NewLine);
            string ssdinos = "";
            foreach (var item in _simpleSpawnerItems)
            {
                ssdinos = ssdinos + item + ";";
            }

            ssdinos = ssdinos.TrimEnd(';');
            sb.Append(ssdinos + Environment.NewLine);
            return sb.ToString();
        }

        protected virtual bool IsRelevantPrimalItem(string name)
        {
            if (name.StartsWith(@"PrimalItemResource_") ||
                name.StartsWith(@"PrimalItemConsumable_") ||
                name.StartsWith(@"PrimalItemArmor_"))
            {
                return true;
            }

            return false;
        }

        protected virtual string GetPrimalItemLine(string parseText)
        {
            string pattern = @".*Blueprint'(?<juice>.*)'.*";
            Regex reg = new Regex(pattern);

            Match match = reg.Match(parseText);
            if (match.Success)
            {
                foreach (Group group in match.Groups)
                {
                    if (group.Name.Equals("juice"))
                    {
                        return group.Value;
                    }
                }
            }

            return "";
        }

        public static ISpawnCodeGenerator GetGeneratorForMod(Mod mod)
        {
            if (!mod.IsAnalysed)
            {
                throw new Exception("Mod should be analysed");
            }

            switch (mod.ModID)
            {
                //case "111111111":
                //    return new PrimitivePlusSpawnCodeGenerator();

                default:
                    return new StandardSpawnCodeGenerator();
            }
        }
    }

    public class StandardSpawnCodeGenerator : BaseSpawnCodeGenerator, ISpawnCodeGenerator
    {
        public StandardSpawnCodeGenerator()
        {
        }
    }

    public class PrimitivePlusSpawnCodeGenerator : BaseSpawnCodeGenerator, ISpawnCodeGenerator
    {
        public PrimitivePlusSpawnCodeGenerator()
        {
        }

        protected override string GetItemSpawnCodes(string[] files, Options options)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerItemSpawncodes);

            //Problem to solve is the repition of \Content\Mods\

            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.StartsWith("PrimalItem"))
                {
                  
                    string str7 = "admincheat GiveItem \"Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 1 1 0";

                    sb.Append(str7 + Environment.NewLine);

                    if (IsRelevantPrimalItem(withoutExtension) && options.SPlus)
                    {
                        string itemText = GetPrimalItemLine(str7);
                        _sPlusItems.Add(itemText);
                    }
                }
            }

            return sb.ToString();
        }
    }
}