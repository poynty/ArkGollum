namespace ArkGollum
{
    public abstract class ModeProcessorBase
    {
        public string FolderPath { get; set; }

        protected Options _options;
        protected List<string> _noPreciousInTheseDirectories = new List<string>();

        public virtual bool VerifyFolder(out string reason)
        {
            if (!Directory.Exists(FolderPath))
            {
                reason = $"Cannot find folder {FolderPath}";
                return false;
            }

            if (Directory.GetFiles(FolderPath).Length == 0)
            {
                reason = "Specified folder contains no files or sub folders";
                return false;
            }

            //Test if something is in the output option
            if (!string.IsNullOrEmpty(_options.Output))
            {
                //There is, does it exist, if not creat?
                if (!Directory.Exists(_options.Output))
                {
                    try
                    {
                        Directory.CreateDirectory(_options.Output);
                    }
                    catch (Exception ex)
                    {
                        reason = "Failed to create output directory";
                        return false;
                    }
                }

                _options.OutputSpecified = true;
            }

            reason = string.Empty;
            return true;
        }
    }
}