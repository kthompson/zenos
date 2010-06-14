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
        public MemberCompiler MemberCompiler { get; private set; }
        public List<CompilerStage> Stages { get; private set; }

        public Compiler(MemberCompiler mc)
            : base(null)
        {
            this.MemberCompiler = mc;
            this.Stages = new List<CompilerStage>();
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
