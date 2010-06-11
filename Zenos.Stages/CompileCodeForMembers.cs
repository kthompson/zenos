using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class CompileCodeForMembers : MemberCompilerStage
    {
        public CompileCodeForMembers(MemberCompiler compiler)
            : base(compiler)
        {
        }

        public override IMemberContext Compile(IMemberContext context)
        {
            foreach (var code in context.CodeContexts)
            {
                using(var writer = new StreamWriter(File.OpenWrite(context.OutputFile)))
                {
                    writer.Write(code.Text.ToString());
                    writer.WriteLine(); 
                    writer.WriteLine();
                }
            }

            return base.Compile(context);
        }
        
    }
}
