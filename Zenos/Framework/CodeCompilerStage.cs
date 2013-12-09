using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Zenos.Framework
{
    public abstract class CodeCompilerStage : MemberCompilerStage, ICodeCompilerStage
    {
        public override void Compile(ITypeContext context)
        {
            foreach(var cc in context.MethodContexts)
                this.Compile(cc);
        }

        public abstract void Compile(IMethodContext context);
    }
}