////
////
//// Parts of this code are adapted from ArkCodeGenerator found at https://
////
////




using System.Text;
using System.Text.RegularExpressions;
using ArkGollum.CodeGenerators;

namespace ArkGollum
{
    public class Mod : BaseMod, IMod
    {
        private readonly string modNameRegex = ".*?\\0\\0\\0(?<modName>.*?)\\0.*";
        private Options _options;

        public Mod(string path, Options options)
        {
            ModPath = path;
            _options = options;
        }

        public override bool GenerateOutput()
        {
            if (!this.IsAnalysed)
            {
                throw new Exception("Gollum must analyse the folder first by calling .AnalyseMod()");
            }

            string textData = MakeSpawnCodes();



            try
            {
                textData = GetFileHeaderString() + GetModInfoString() + textData;

                string file = "";
                if(!_options.OutputSpecified)
                {
                    file = Path.Combine(this.ModPath, GollumFileName);
                }
                else
                {
                    file = Path.Combine(_options.Output, GollumFileName);
                }

                if(File.Exists(file))
                {
                    File.Delete(file);
                }

                File.WriteAllText(file, textData);

            }catch (Exception ex) {
                //logging in future
                Console.WriteLine(ex.ToString());
                return false;
            }
          

            return true;
        }

        private string MakeSpawnCodes()
        {
            ISpawnCodeGenerator spawnCodeGenerator = BaseSpawnCodeGenerator.GetGeneratorForMod(this);
            string[] files = Directory.GetFiles(ModPath, "*", SearchOption.AllDirectories);
            return spawnCodeGenerator.ProduceOutput(files, _options);

            //StringBuilder output = new StringBuilder();

            //List<string> simpleSpawnersDinos = new List<string>();
            //List<string> sPlusThings = new List<string>();

        
            //string str2 = "\n---------------------------------------------------------------------------------Engram Names------------------------------------------------------------------------------------\n";
            //string str3 = "\n---------------------------------------------------------------------------------Item Spawncodes---------------------------------------------------------------------------------\n";
            //string str4 = "\n---------------------------------------------------------------------------------Creature Spawncodes-----------------------------------------------------------------------------\n";
            //string str5 = "\n---------------------------------------------------------------------------------Tamed Creature Spawncodes-----------------------------------------------------------------------\n";
            //string x1   = "\n---------------------------------------------------------------------------------S+ Pull Resources-------------------------------------------------------------------------------\n";
            //string x2   = "\n---------------------------------------------------------------------------------Simple Spawners---------------------------------------------------------------------------------\n";
            //string[] files = Directory.GetFiles(ModPath, "*", SearchOption.AllDirectories);

           
            //output.Append(str2);

            //foreach (string path in files)
            //{
            //    string withoutExtension = Path.GetFileNameWithoutExtension(path);
            //    if (withoutExtension.StartsWith("EngramEntry"))
            //    {
            //        string str6 = withoutExtension + "_C";

            //        output.Append(str6 + Environment.NewLine);
            //    }
            //}

            //output.Append(str3 + Environment.NewLine);

            //foreach (string path in files)
            //{
            //    string withoutExtension = Path.GetFileNameWithoutExtension(path);
            //    if (withoutExtension.StartsWith("PrimalItem"))
            //    {
            //        string str7 = "admincheat GiveItem \"Blueprint'" + path.Substring(path.IndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 1 1 0";

            //        output.Append(str7 + Environment.NewLine);

            //        if (IsRelevantPrimalItem(withoutExtension) && _options.SPlus)
            //        {
            //            string itemText = GetPrimalItemLine(str7);
            //            sPlusThings.Add(itemText);
            //        }
            //    }
            //}

            //output.Append(str4 + Environment.NewLine);

            //foreach (string path in files)
            //{
            //    string withoutExtension = Path.GetFileNameWithoutExtension(path);
            //    if (withoutExtension.Contains("Character_BP"))
            //    {
            //        string str8 = "admincheat SpawnDino \"Blueprint'" + path.Substring(path.IndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 500 0 0 120";
            //        simpleSpawnersDinos.Add("Blueprint'" + path.Substring(path.IndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'");
            //        output.Append(str8 + Environment.NewLine);
            //    }
            //}

            //output.Append(str5 + Environment.NewLine);

            //foreach (string path in files)
            //{
            //    string str9 = Path.GetFileNameWithoutExtension(path) + "_C";
            //    if (str9.Contains("Character_BP"))
            //    {
            //        string str10 = "admincheat GMSummon \"" + str9 + "\" 120";

            //        output.Append(str10 + Environment.NewLine);
            //    }
            //}

            //if (_options.SPlus&&sPlusThings.Count>0)
            //{
            //    output.Append(x1 + Environment.NewLine);
            //    //Generate s+ pull list
            //    StringBuilder sb = new StringBuilder();
            //    string sPlus = "PullResourceAdditions=";
            //    foreach (var resource in sPlusThings)
            //    {
            //        sPlus = sPlus + resource + ",";
            //    }
            //    sPlus = sPlus.TrimEnd(',');
            //    output.Append(sPlus + Environment.NewLine);
            //}

            //if (_options.SimpleSpawners&&simpleSpawnersDinos.Count>0)
            //{
            //    output.Append(x2 + Environment.NewLine);
            //    string ssdinos = "";
            //    foreach (var item in simpleSpawnersDinos)
            //    {
            //        ssdinos = ssdinos + item + ";";
            //    }

            //    ssdinos = ssdinos.TrimEnd(';');
            //    output.Append(ssdinos+Environment.NewLine);
            //}      

            //return output.ToString().Trim();
        }

        public override bool AnalyseMod()
        {
            //Find mod name
            if (!File.Exists(ModPath + Path.DirectorySeparatorChar + "mod.info"))
            {
                return false;
                //throw new Exception("Gollum found no mod.info file!");
            }
            string modText = File.ReadAllText(ModPath + Path.DirectorySeparatorChar + "mod.info");
            ModName = GetModName(modText);

            //Find Mod id
            string[] dirSplit = ModPath.Split('\\');
            ModID = dirSplit[dirSplit.Length - 1];

            //Find steam link
            SteamWorkshopURL = _steamWorkshopBaseURL + ModID;

            GollumFileName = ModName + "_" + ModID + ".txt";

            IsAnalysed = true;
            return true;
        }

        public override bool IsValidMod()
        {
            string[] files = Directory.GetFiles(ModPath, "*", SearchOption.AllDirectories);
            int fileCount = 0;
            int amtFiles = files.Length;

            foreach (string file in files)
            {
                if (Path.GetFileNameWithoutExtension(file).StartsWith("PrimalGameData"))
                    return true;
                if (fileCount == amtFiles - 1)
                    return false;
                fileCount++;
            }
            return false;
        }

        protected override string GetModName(string text)
        {
            Regex regex = new Regex(modNameRegex);
            var result = regex.Match(text);
            if (result.Success)
            {
                return result.Groups["modName"].Value;
            }
            return "NOT FOUND";
        }

        public string GetModInfoString()
        {
            if(!IsAnalysed)
            {
                throw new Exception("Cannot provide info for un-analyzed mod");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Name: {ModName}");
            sb.AppendLine($"ID: {ModID}");
            sb.AppendLine($"URL: {SteamWorkshopURL}");
            sb.AppendLine("");

            return sb.ToString();
        }
    }
}