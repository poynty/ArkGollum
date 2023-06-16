namespace ArkGollum
{
    public interface IModeProcessor
    {
        public bool VerifyFolder(out string reason);
        public void Process();
        public void Purge();
    }
}