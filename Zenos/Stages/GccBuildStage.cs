using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class GccBuildStage : CompilerStage
    {
        public override void Compile(IAssemblyContext context)
        {
            var cmd = new StringBuilder("gcc -Wall -shared -o ");
            cmd.Append(context.OutputFile);

            foreach (var code in context.Types.SelectMany(t => t.MethodContexts))
                cmd.AppendFormat(" {0}", code.OutputFile);

            string output;
            string error;
            if (Helper.Execute(cmd.ToString(), out error, out output) == 0)
            {
                if (!string.IsNullOrEmpty(error))
                    Helper.Break();

                return;
            }

            Helper.Stop(new ApplicationException(error + output));
        }
    }
}
