namespace ArkGollum
{
    public class ModModeProcessor : ModeProcessorBase, IModeProcessor
    {
        private readonly string pathEnd = "ShooterGame\\Content\\Mods";
        

        public ModModeProcessor(Options options)
        {
            FolderPath = Path.Combine(options.TargetFolder, pathEnd);
            _options = options;
        }

        public void Process()
        {
            List<IMod> modFolders = DirectoryAnalyser.GetModFolders(FolderPath,_options);

            int total = modFolders.Count;
            int i = 0;
            int valid = 0;

            foreach (BaseMod modFolder in modFolders)
            {
                i++;
                if (modFolder.IsValidMod())
                {
                    if (modFolder.AnalyseMod())
                    {
                        modFolder.GenerateOutput();
                        valid++;
                    }
                    else
                    {
                        continue;
                    }
                }

                double pProgress = ((double)i / (double)total) * 100;
                int iProgress = Convert.ToInt32(pProgress);
                double pValid = ((double)valid / (double)total) * 100;
                int iValid = Convert.ToInt32(pValid);
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"\bGollum has searched in {iProgress}% of folders and has found some precious in {iValid}% of those folders.");
            }
        }

        public override bool VerifyFolder(out string reason)
        {
            bool flag = base.VerifyFolder(out reason);

            return flag;
        }

        public void Purge()
        {
            List<IMod> modFolders = DirectoryAnalyser.GetModFolders(FolderPath, _options);

            int total = modFolders.Count;
            int i = 0;
            int purged = 0;

            foreach (BaseMod modFolder in modFolders)
            {
                i++;
                if (modFolder.IsValidMod())
                {
                    if (modFolder.AnalyseMod())
                    {
                        string file = Path.Combine(modFolder.ModPath, modFolder.GollumFileName);
                        if(File.Exists(file))
                        {
                            try
                            {
                                File.Delete(file);
                                purged++;
                            }catch(Exception e)
                            {
                                //Do something with this later
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                }

                double pProgress = ((double)i / (double)total) * 100;
                int iProgress = Convert.ToInt32(pProgress);
                double pPurged = ((double)purged / (double)total) * 100;
                int iPurged = Convert.ToInt32(pPurged);
                Console.SetCursorPosition(0, Console.CursorTop);
                //TODO accurately convey the amount of files deleted
                Console.Write($"\b Gollum has searched in {iProgress}% of folders and has purged his work in {iPurged}% of those folders.");
            }
        }

        
    }
}