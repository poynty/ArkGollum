namespace ArkGollum
{
    public abstract class ModeProcessorBase
    {
        public string FolderPath { get; set; }

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
            reason = string.Empty;
            return true;
        }
    }
}