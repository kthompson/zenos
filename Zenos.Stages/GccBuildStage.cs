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

        public override ICompilerContext Compile(ICompilerContext context, Mono.Cecil.AssemblyDefinition assembly)
        {
            return base.Compile(context, assembly);
        }

        public override ICompilerContext Compile(ICompilerContext context, Mono.Cecil.AssemblyNameDefinition assemblyName)
        {
            return base.Compile(context, assemblyName);
        }

        public override ICompilerContext Compile(ICompilerContext context, Mono.Cecil.ModuleDefinition module)
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

                return null;
            }

            Helper.Stop(() => new ApplicationException(error + output));
            return null;
        }
    }
}
