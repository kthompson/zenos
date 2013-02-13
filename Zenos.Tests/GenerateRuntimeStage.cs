using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Collections.Generic;
using Zenos.Core;
using Zenos.Framework;
using System.Linq;

namespace Zenos.Tests
{
    public class GenerateRuntimeStage : MemberCompilerStage
    {
        public override void Compile(IMemberContext context, MethodDefinition method)
        {
            var cc = context.CreateCompilationContext();

            var arguments = GetMethodArguments(context);
            cc.OutputFile = "runtime_".Append(method.Name, "_").AppendRandom(32, ".c");

            using (var runtime = new StreamWriter(File.OpenWrite(cc.OutputFile)))
            {
                var function = string.Format("{0}({1})", method.Name, string.Join(", ", arguments));

                var printf = GetPrintFormat(method, ref function);

                runtime.WriteLine("#include <stdio.h>");
                runtime.WriteLine("#include <stdbool.h>");
                runtime.WriteLine("#include <stdlib.h>");
                runtime.WriteLine();
                WriteMethodSignature(runtime, method);
                runtime.WriteLine();
                runtime.WriteLine("int main(int argc, char** argv)");
                runtime.WriteLine("{");
                runtime.WriteLine("	printf(\"{0}\\n\", {1});", printf, function);
                runtime.WriteLine("	return 0;");
                runtime.WriteLine("}");
            }


            base.Compile(context, method);
        }

        private static void WriteMethodSignature(StreamWriter runtime, MethodReference method)
        {
            WriteType(runtime, method.ReturnType);
            runtime.Write(" {0}(", method.Name);
            WriteParameters(runtime, method.Parameters);
            runtime.WriteLine(");");
        }

        private static void WriteParameters(StreamWriter runtime, Collection<ParameterDefinition> parameters)
        {
            var length = parameters.Count;
            for (var i = 0; i < length; i++)
            {
                WriteParameter(runtime, parameters[i]);
                if (i < length - 1)
                    runtime.Write(", ");
            }
        }

        private static void WriteParameter(StreamWriter runtime, ParameterReference parameterDefinition)
        {
            WriteType(runtime, parameterDefinition.ParameterType);
            runtime.Write(" {0}", parameterDefinition.Name);
        }

        private static void WriteType(TextWriter runtime, MemberReference returnType)
        {
            var type = string.Empty;
            switch (returnType.FullName)
            {
                case "System.Int32":
                    type = "int";
                    break;
                case "System.Boolean":
                    type = "bool";
                    break;
                case "System.Char":
                    type = "char";
                    break;
                case "System.Single":
                    type = "float";
                    break;
                default:
                    Helper.NotSupported(string.Format("Referenced type is not supported: {0}", returnType.FullName));
                    break;
            }

            runtime.Write(type);
        }

        private static IEnumerable<string> GetMethodArguments(IMemberContext context)
        {
            var root = context.Context as TestContext;
            Helper.IsNotNull(root);
            return root.Arguments.Select(o => o.ToString());
        }

        private static string GetPrintFormat(MethodReference method, ref string function)
        {
            string printf;
            string returnType;
            switch (method.ReturnType.Name.ToLower())
            {
                case "single":
                    printf = "%f";
                    returnType = "float";
                    break;
                case "int32":
                    printf = "%d";
                    returnType = "long";
                    break;
                case "boolean":
                    printf = "%s";
                    returnType = "bool";
                    function += " ? \"True\" : \"False\"";
                    break;
                case "char":
                    printf = "%c";
                    returnType = "char";
                    break;
                default:
                    printf = "%d";
                    returnType = "long";
                    Helper.Break();
                    break;
            }
            return printf;
        }
    }
}