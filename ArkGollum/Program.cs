using CommandLine;
using CommandLine.Text;

namespace ArkGollum
{
    public class Program
    {
        private static Mode currentMode;

        public static void Main(string[] args)
        {
            

            Options options = new Options();
            var result = Parser.Default.ParseArguments<Options>(args);
     
            if (result.Errors.Count() == 0)
            {
                options = result.Value as Options;
                currentMode = options.Mode;

                IModeProcessor modeProcessor;
                string errorReason = "";
                switch (currentMode)
                {
                    case Mode.MOD:
                        modeProcessor = new ModModeProcessor(options);
                        if (modeProcessor.VerifyFolder(out errorReason))
                        {
                            if (options.Purge)
                            {
                                modeProcessor.Purge();
                            }
                            else { modeProcessor.Process(); }
                        }
                        else
                        {
                            Console.WriteLine($"FATAL: {errorReason}");
                        }
                        break;

                    case Mode.WORKSHOP:
                        Console.WriteLine("Mode not implemented yet.");
                        break;

                    case Mode.BASE:
                        Console.WriteLine("Mode not implemented yet.");
                        break;
                }
            }
            else
            {
                Console.WriteLine(HelpText.RenderUsageText<Options>(result));
            }
        }
    }
}