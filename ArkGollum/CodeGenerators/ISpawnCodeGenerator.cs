namespace ArkGollum.CodeGenerators
{
    public interface ISpawnCodeGenerator
    {
        public string ProduceOutput(string[] files, Options options);
    }
}