using System.Text;

namespace ArkGollum.CodeGenerators
{
    public class ZytharianCreaturesSpawnCodeGenerator : BaseSpawnCodeGenerator, ISpawnCodeGenerator
    {
        private List<string> arkOmegaCodes = new List<string>();
        private List<string> zytharianCodes = new List<string>();
        private List<string> arkTamedOmegaCodes = new List<string>();
        private List<string> zytharianTamedCodes = new List<string>();

        protected readonly string bannerOmegaCreatureSpawnCodes = "\n---------------------------------------------------------------------------------Creature Spawncodes for Ark Omega creatures in Zytharian Creatures------------------------------\n";
        protected readonly string bannerTamedOmegaCreatureSpawnCodes = "\n--------------------------------------------------------------------------------- Tamed Creature Spawncodes for Ark Omega creatures in Zytharian Creatures-----------------------\n";

        public ZytharianCreaturesSpawnCodeGenerator()
        {
        }

        public override string ProduceOutput(string[] files, Options options)
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
                foreach (var line in zytharianCodes)
                {
                    output.Append(line + Environment.NewLine);
                }

                output.Append(bannerOmegaCreatureSpawnCodes);
                foreach (var line in arkOmegaCodes)
                {
                    output.Append(line + Environment.NewLine);
                }
            }

            if (tamedCreatureCount > 0)
            {
                output.Append(bannerTamedCreatureSpawnCodes);
                foreach (var line in zytharianTamedCodes)
                {
                    output.Append(line + Environment.NewLine);
                }

                output.Append(bannerTamedOmegaCreatureSpawnCodes);
                foreach (var line in arkTamedOmegaCodes)
                {
                    output.Append(line + Environment.NewLine);
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

        protected override void FindCreatures(string[] files, Options options)
        {
            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.Contains("Character_BP"))
                {
                    creatureCount++;
                    string str8 = "admincheat SpawnDino \"Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + $"'\" 500 0 0 {options.Level}";

                    if (withoutExtension.Contains("AO"))
                    {
                        arkOmegaCodes.Add(str8);
                    }
                    else
                    {
                        zytharianCodes.Add(str8);
                    }

                    _simpleSpawnerItems.Add("Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'");
                }
            }
        }

        protected override void FindTamedCreatures(string[] files, Options options)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.Append(bannerTamedCreatureSpawnCodes);

            foreach (string path in files)
            {
                tamedCreatureCount++;
                string str9 = Path.GetFileNameWithoutExtension(path) + "_C";
                if (str9.Contains("Character_BP"))
                {
                    string str10 = "admincheat GMSummon \"" + str9 + $"\" {options.Level}";

                    if (str9.Contains("AO"))
                    {
                        arkTamedOmegaCodes.Add(str10);
                    }
                    else
                    {
                        zytharianTamedCodes.Add(str10);
                    }
                }
            }
        }
    }
}