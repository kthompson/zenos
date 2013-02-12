using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class WriteCodeToDisk : CodeCompilerStage
    {
        public override void Compile(ICompilationContext context, MethodBody body)
        {
            switch (context.CodeType)
            {
                case CodeType.Assembler:
                    context.OutputFile = body.Method.Name.ToFileName().AppendRandom("_", 32, ".s");
                    break;
                case CodeType.C:
                    context.OutputFile = body.Method.Name.ToFileName().AppendRandom("_", 32, ".c");
                    break;
                default:
                    Helper.Break();
                    break;
            }

            using (var writer = new StreamWriter(File.OpenWrite(context.OutputFile)))
            {
                writer.Write(context.Text.ToString());
                writer.WriteLine();
                writer.WriteLine();
            }
        }
    }
}
