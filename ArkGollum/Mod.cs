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
                if (Path.GetFileNameWithoutExtension(file).StartsWith("PrimalGameData")||Path.GetFileNameWithoutExtension(file).StartsWith("PGD_"))
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