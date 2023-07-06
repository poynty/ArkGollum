using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkGollum.CodeGenerators
{
    public class DebugModCodeGenerator:BaseSpawnCodeGenerator,ISpawnCodeGenerator
    {
        public DebugModCodeGenerator()
        {
            
        }

        public override string ProduceOutput(string[] files, Options options)
        {
            return base.ProduceOutput(files, options);
        }

       
    }
}
