namespace ArkGollum.CodeGenerators
{
    public interface ISpawnCodeGenerator
    {
        public bool HasSearchedForCodes { get; set; }

        public string ProduceOutput(string[] files, Options options);
        public void FindCodes(string[] files, Options options);

        public bool HasCodesToOutput();


    }
}