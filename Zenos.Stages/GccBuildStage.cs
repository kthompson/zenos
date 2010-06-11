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
        public GccBuildStage(Compiler compiler)
            : base(compiler)
        {
        }

        public override ICompilerContext Compile(ICompilerContext context)
        {
            var cmd = new StringBuilder("gcc -Wall -o ");
            cmd.Append(context.OutputFile);
            

            foreach (var member in context.Members)
                cmd.AppendFormat(" {0}", member.OutputFile);

            string output;
            string error;
            if (Helper.Execute(cmd.ToString(), out error, out output) == 0)
            {
                if (!string.IsNullOrEmpty(error))
                    Helper.Break();

                return null;
            }

            Helper.Stop(() => new ApplicationException(error + output));
            return null;
        }
    }
}
