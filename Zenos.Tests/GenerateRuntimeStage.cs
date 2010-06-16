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
        public GenerateRuntimeStage(MemberCompiler compiler)
            : base(compiler)
        {
        }

        public override IMemberContext Compile(IMemberContext context, MethodDefinition method)
        {
            var cc = CreateRuntime(context, method);

            context.CodeContexts.Add(cc);

            return base.Compile(context, method);
        }

        private static ICodeContext CreateRuntime(IMemberContext context, MethodReference method)
        {
            ICodeContext cc = new CodeContext(context, CodeType.C);

            var arguments = GetMethodArguments(context);
            cc.OutputFile = "runtime_".Append(method.Name, "_").AppendRandom(32, ".c");

            using (var runtime = new StreamWriter(File.OpenWrite(cc.OutputFile)))
            {
                //string function = "setup_stack(stack_base)";
                var function = string.Format("{0}({1})", method.Name, string.Join(", ", arguments));
                string returnType;

                var printf = GetPrintFormat(method, ref function);

                runtime.WriteLine("#include <stdio.h>");
                runtime.WriteLine("#include <stdbool.h>");
                runtime.WriteLine("#include <stdlib.h>");
                //runtime.WriteLine();
                //runtime.WriteLine(string.Format("{0} setup_stack(char*);", returnType));
                WriteMethodSignature(runtime, method);
                runtime.WriteLine();
                runtime.WriteLine();
                runtime.WriteLine("int main(int argc, char** argv)");
                runtime.WriteLine("{");
                //runtime.WriteLine("	int stack_size = (16 * 4096);");
                //runtime.WriteLine("	char* stack_top = malloc(stack_size);");
                //runtime.WriteLine("	char* stack_base = stack_top + stack_size;");
                runtime.WriteLine(string.Format("	printf(\"{0}\\n\", {1});", printf, function));
                //runtime.WriteLine("	free(stack_top);");
                runtime.WriteLine("	return 0;");
                runtime.WriteLine("}");
            }

            return cc;
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

        private static void WriteParameter(StreamWriter runtime, ParameterDefinition parameterDefinition)
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
                    printf = "%.3f";
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