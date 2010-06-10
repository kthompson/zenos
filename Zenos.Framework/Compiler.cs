using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class Compiler : CompilerStage
    {
        public List<CompilerStage> Stages { get; private set; }

        public Compiler()
        {
            this.Stages = new List<CompilerStage>();
        }

        public override ICompilerContext Compile(ICompilerContext context)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current));
        }

        public override ICompilerContext Compile(ICompilerContext context, AssemblyDefinition assembly)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, assembly));
        }

        public override ICompilerContext Compile(ICompilerContext context, AssemblyNameDefinition assemblyName)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, assemblyName));
        }

        public override ICompilerContext Compile(ICompilerContext context, ModuleDefinition module)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, module));
        }
    }
}
