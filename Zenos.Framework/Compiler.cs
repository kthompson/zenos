using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace Zenos.Framework
{
    public class Compiler : CompilerStage
    {
        public List<ICompilerStage> Stages { get; private set; }

        public Compiler(IEnumerable<ICompilerStage> stages)
        {
            this.Stages = new List<ICompilerStage>(stages);
        }

        public override void Compile(ICompilerContext context, AssemblyDefinition assembly)
        {
            this.Stages.ForEach(stage => stage.Compile(context, assembly));
        }

        public override void Compile(ICompilerContext context, AssemblyNameDefinition assemblyName)
        {
            this.Stages.ForEach(stage => stage.Compile(context, assemblyName));
        }

        public override void Compile(ICompilerContext context, ModuleDefinition module)
        {
            this.Stages.ForEach(stage => stage.Compile(context, module));
        }
    }
}
