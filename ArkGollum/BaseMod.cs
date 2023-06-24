using System.Text.RegularExpressions;

namespace ArkGollum
{
    public abstract class BaseMod
    {
        protected readonly string modNameRegex = ".*?\\0\\0\\0(?<modName>.*?)\\0.*";
        protected Options _options;
        protected readonly string _steamWorkshopBaseURL = @"https://steamcommunity.com/sharedfiles/filedetails/?id=";
        
        public string ModPath { get; set; } = "";
        public string ModName { get; set; } = "";
        public string ModID { get; set; } = "";

        public string GollumFileName { get; set; } = "";

        public bool IsAnalysed { get; set; } = false;

        public string SteamWorkshopURL { get; set; } = "";

        public abstract bool GenerateOutput();

        public abstract bool IsValidMod();

        public abstract bool AnalyseMod();

        protected virtual string GetModName(string fileText)
        {
            return "UNKNOWN";
        }

        protected virtual bool IsRelevantPrimalItem(string name)
        {
            if(name.StartsWith(@"PrimalItemResource_")||
                name.StartsWith(@"PrimalItemConsumable_")||
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

        public virtual string GetFileHeaderString()
        {
            string str0 = "ARK Gollum, he finds the precious so you don't have to.\n\n";
            string str1 = "\nInspired by and developed from the core of ARKMod.net's ARK Code Generator. Visit https://arkmod.net/ for details.\nHappy ARKing!\n\n\n";

            return str0 + str1;
        }
    }
}
