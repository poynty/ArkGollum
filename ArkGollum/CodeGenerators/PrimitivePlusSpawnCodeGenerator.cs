using System.Text;

namespace ArkGollum.CodeGenerators
{
    public class PrimitivePlusSpawnCodeGenerator : BaseSpawnCodeGenerator, ISpawnCodeGenerator
    {
        public PrimitivePlusSpawnCodeGenerator()
        {
        }

        protected override string GetItemSpawnCodes(string[] files, Options options)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(bannerItemSpawncodes);

            //Problem to solve is the repition of \Content\Mods\

            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.StartsWith("PrimalItem"))
                {

                    string str7 = "admincheat GiveItem \"Blueprint'" + path.Substring(path.LastIndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 1 1 0";

                    sb.Append(str7 + Environment.NewLine);

                    if (IsRelevantPrimalItem(withoutExtension) && options.SPlus)
                    {
                        string itemText = GetPrimalItemLine(str7);
                        _sPlusItems.Add(itemText);
                    }
                }
            }

            return sb.ToString();
        }
    }
}