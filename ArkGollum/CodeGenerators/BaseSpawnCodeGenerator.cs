using System.Text;
using System.Text.RegularExpressions;

namespace ArkGollum.CodeGenerators
{
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

        protected int engramCount = 0;
        protected int itemCount = 0;
        protected int creatureCount = 0;
        protected int tamedCreatureCount = 0;

        protected List<string> engrams = new List<string>();
        protected List<string> items = new List<string>();
        protected List<string> creatures = new List<string>();
        protected List<string> tamedCreatures = new List<string>();

        public bool HasSearchedForCodes { get; set; } = false;

        public virtual string ProduceOutput(string[] files, Options options)
        {
            if (!HasSearchedForCodes)
            {
                throw new Exception("Can only produce output after searching for codes");
            }

            if (!HasCodesToOutput())
            {
                throw new Exception("Nothing to output");
            }

            StringBuilder output = new StringBuilder();

            if (engramCount > 0)
            {
                output.Append(bannerEngramNames);
                foreach (string engram in engrams)
                {
                    output.AppendLine(engram);
                }
            }

            if (itemCount > 0)
            {
                output.Append(bannerItemSpawncodes);
                foreach (string item in items)
                {
                    output.AppendLine(item);
                }
            }

            if (creatureCount > 0)
            {
                output.Append(bannerCreatureSpawnCodes);
                foreach (string creature in creatures)
                {
                    output.AppendLine(creature);
                }
            }

            if (tamedCreatureCount > 0)
            {
                output.Append(bannerTamedCreatureSpawnCodes);
                foreach (string tameCreature in tamedCreatures)
                {
                    output.AppendLine(tameCreature);
                }
            }

            if (options.SPlus && _sPlusItems.Count > 0)
            {
                output.Append(GetSPlus());
            }

            if (options.SimpleSpawners && _simpleSpawnerItems.Count > 0)
            {
                output.Append(GetSimpleSpawners());
            }

            return output.ToString();
        }

        public virtual bool HasCodesToOutput()
        {
            return engramCount + itemCount + creatures.Count + tamedCreatureCount > 0;
        }

        public virtual void FindCodes(string[] files, Options options)
        {
            HasSearchedForCodes = false;

            engramCount = 0;
            itemCount = 0;
            creatureCount = 0;
            tamedCreatureCount = 0;

            engrams.Clear();
            items.Clear();
            creatures.Clear();
            tamedCreatures.Clear();

            FindEngrams(files);
            FindItems(files, options);
            FindCreatures(files, options);
            FindTamedCreatures(files, options);

            HasSearchedForCodes = true;
        }

        protected virtual void FindEngrams(string[] files)
        {
            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.StartsWith("EngramEntry"))
                {
                    string str6 = withoutExtension + "_C";

                    engramCount++;
                    engrams.Add(str6);
                }
            }
        }

        protected virtual void FindItems(string[] files, Options options)
        {
            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.StartsWith("PrimalItem"))
                {
                    itemCount++;
                    string str7 = "admincheat GiveItem \"Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 1 1 0";
                    items.Add(str7);

                    if (IsRelevantPrimalItem(withoutExtension) && options.SPlus)
                    {
                        string itemText = GetPrimalItemLine(str7);
                        _sPlusItems.Add(itemText);
                    }
                }
            }
        }

        protected virtual void FindCreatures(string[] files, Options options)
        {
            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.Contains("Character_BP"))
                {
                    creatureCount++;
                    string str8 = "admincheat SpawnDino \"Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + $"'\" 500 0 0 {options.Level}";
                    _simpleSpawnerItems.Add("Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'");
                    creatures.Add(str8);
                }
            }
        }

        protected virtual void FindTamedCreatures(string[] files, Options options)
        {
            foreach (string path in files)
            {
                string str9 = Path.GetFileNameWithoutExtension(path) + "_C";
                if (str9.Contains("Character_BP"))
                {
                    string str10 = "admincheat GMSummon \"" + str9 + $"\" {options.Level}";
                    tamedCreatureCount++;
                    tamedCreatures.Add(str10);
                }
            }
        }

      

      
        protected virtual string GetSPlus()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerSPlus);
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

        protected virtual string GetSimpleSpawners()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerSimpleSpawners);
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
                case "1754846792":
                    return new ZytharianCreaturesSpawnCodeGenerator();

                default:
                    return new StandardSpawnCodeGenerator();
            }
        }
    }
}