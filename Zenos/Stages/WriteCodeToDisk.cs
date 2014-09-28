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
        public override void Compile(IMethodContext context)
        {
            using (var writer = new StreamWriter(File.OpenWrite(context.OutputFile)))
            {
                writer.Write(context.Text.ToString());
                writer.WriteLine();
                writer.WriteLine();

                writer.Write(context.Data.ToString());
                writer.WriteLine();
                writer.WriteLine();

                writer.Write(context.Sections["drectve"].ToString());
                writer.WriteLine();
                writer.WriteLine();
            }
        }
    }
}
