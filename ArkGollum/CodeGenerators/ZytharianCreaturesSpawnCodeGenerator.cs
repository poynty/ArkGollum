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

        protected override string GetCreatureSpawnCodes(string[] files, Options options)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(bannerCreatureSpawnCodes);

            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.Contains("Character_BP"))
                {
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

            sb.Append(bannerCreatureSpawnCodes);
            foreach (var line in zytharianCodes)
            {
                sb.Append(line + Environment.NewLine);
            }

            sb.Append(bannerOmegaCreatureSpawnCodes);
            foreach (var line in arkOmegaCodes)
            {
                sb.Append(line + Environment.NewLine);
            }
            return sb.ToString();
        }

        protected override string GetTamedCreatureSpawnCodes(string[] files, Options options)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(bannerTamedCreatureSpawnCodes);

            foreach (string path in files)
            {
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

            sb.Append(bannerTamedCreatureSpawnCodes);
            foreach (var line in zytharianTamedCodes)
            {
                sb.Append(line + Environment.NewLine);
            }

            sb.Append(bannerTamedOmegaCreatureSpawnCodes);
            foreach (var line in arkTamedOmegaCodes)
            {
                sb.Append(line + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}