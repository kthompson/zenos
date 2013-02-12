using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class CodeQueuingStage : MemberCompilerStage
    {
        public CodeCompiler CodeCompiler { get; private set; }

        public CodeQueuingStage(CodeCompiler codeCompiler)
        {
            CodeCompiler = codeCompiler;
        }

        public override void Compile(IMemberContext context, MethodDefinition method)
        {
            if (!method.HasBody) 
                return;

            var compilationContext = new CompilationContext(context, CodeType.Assembler);
            this.CodeCompiler.Compile(compilationContext, method.Body);
            context.CodeContexts.Add(compilationContext);
        }
    }
}
