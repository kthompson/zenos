using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class Compiler : CompilerStage
    {
        public List<ICompilerStage> Stages { get; }

        public Compiler(IEnumerable<ICompilerStage> stages)
        {
            this.Stages = new List<ICompilerStage>(stages);
        }

        public override void Compile(IAssemblyContext context)
        {
            this.Stages.ForEach(stage => stage.Compile(context));
        }
    }
}
