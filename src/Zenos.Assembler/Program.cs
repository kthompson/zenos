using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CSharpx;
using Xunit.Abstractions;
using Zenos.Tests;
using static Zenos.Framework.AssemblyParsing;

namespace Zenos.Assembler
{
    class Program
    {
        class IO : ITestOutputHelper
        {
            public void WriteLine(string message)
            {
                Console.WriteLine(message);
            }

            public void WriteLine(string format, params object[] args)
            {
                Console.WriteLine(format, args);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int EightArgMethod(int aa, int bb, int cc, int dd, int ee, int ff, int gg, int hh)
        {
            return ee;
        }

        static void Main(string[] args)
        {
            //var str = File.ReadAllText(@"E:\code\AppDev\zenos\tests\Zenos.Assembler.Tests\resources\basic.asm");

            //var result = FParsec.CharParsers.run(pListing, str);
            //if (result.IsSuccess)
            //{
            //    Console.WriteLine(result.ToString());
            //}

            //var tests = new ArgumentTests(new IO());
            //tests.Arg5(1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008);
            EightArgMethod(1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008);

            //TextWriter x;
            //var name = typeof(Program).GetTypeInfo().Assembly.GetName();
            //switch (Parser.Default.ParseArguments<Options>(args))
            //{
            //    case Parsed<Options> options:

            //        break;
            //    case NotParsed<Options> options:
            //        options.Errors.ToList().ForEach(error =>
            //        {
            //            switch (error)
            //            {
            //                case UnknownOptionError err:
            //                    Console.WriteLine($"The option specified is not supported: {err.Token}");
            //                    break;

            //                case MissingRequiredOptionError err:
            //                    Console.WriteLine($"Required argument {err.NameInfo.NameText} was not specified");
            //                    break;

            //                case BadFormatConversionError err:
            //                    Console.WriteLine($"Invalid value for argument: {err.NameInfo.NameText}");
            //                    break;

            //                default:
            //                    Console.WriteLine($"Invalid value for argument: {error}");
            //                    break;
            //            }

            //        });
            //        break;
            //}


        }
    }

    class Options
    {
        [Option('i', "input", Required = true,
            HelpText = "Input files to be processed.")]
        public IEnumerable<string> InputFiles { get; set; }

        // Omitting long name, default --verbose
        [Option(HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }
}
