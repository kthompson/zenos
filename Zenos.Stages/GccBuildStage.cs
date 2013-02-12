using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class GccBuildStage : CompilerStage
    {
        public override void Compile(ICompilerContext context, Mono.Cecil.ModuleDefinition module)
        {
            var cmd = new StringBuilder("gcc -Wall -o ");
            cmd.Append(context.OutputFile);

            foreach (var code in context.Members.SelectMany(m => m.CodeContexts))
                cmd.AppendFormat(" {0}", code.OutputFile);

            string output;
            string error;
            if (Helper.Execute(cmd.ToString(), out error, out output) == 0)
            {
                if (!string.IsNullOrEmpty(error))
                    Helper.Break();

                return;
            }

            Helper.Stop(() => new ApplicationException(error + output));
        }
    }
}
