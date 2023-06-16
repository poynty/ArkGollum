namespace ArkGollum
{
    public static class DirectoryAnalyser
    {
        public static List<IMod> GetModFolders(string modFolderPath,Options options)
        {
            List<IMod> retList = new List<IMod>();
            string[] result = Directory.GetDirectories(modFolderPath);
            foreach (var folder in result)
            {
                Mod mod = new Mod(folder,options);
                retList.Add(mod);
            }
            return retList;
        }
    }
}