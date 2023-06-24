namespace ArkGollum
{
    public static class DirectoryAnalyser
    {
        public static List<IMod> GetModFolders(string modFolderPath, Options options, string debugModId = "")
        {
            List<IMod> retList = new List<IMod>();
            string[] result = Directory.GetDirectories(modFolderPath);
            foreach (var folder in result)
            {
                if(!string.IsNullOrEmpty(debugModId))
                {
                    if(folder.Contains(debugModId))
                    {
                        ModDebugger mDebug = new ModDebugger(folder, options);
                        retList.Add(mDebug);
                        continue;
                    }
                }
                Mod mod = new Mod(folder,options);
                retList.Add(mod);
            }
            return retList;
        }
    }
}