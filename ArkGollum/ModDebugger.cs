using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArkGollum
{
    public class ModDebugger:BaseMod,IMod
    {
        public ModDebugger(string path, Options options)
        {
            ModPath = path;
            _options = options;
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

        public override bool GenerateOutput()
        {
            throw new NotImplementedException();
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
    }
}
