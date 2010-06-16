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
        public CodeQueuingStage(MemberCompiler compiler)
            : base(compiler)
        {
        }

        public override IMemberContext Compile(IMemberContext context, TypeDefinition type)
        {
            if (type.Methods.Count == 0)
                return context;

            return type.Methods.Aggregate(context, (current, method) => this.MemberCompiler.Compile(current, method));
        }

        public override IMemberContext Compile(IMemberContext context, MethodDefinition method)
        {
            if (method.HasBody)
            {
                ICodeContext cc = new CodeContext(context, CodeType.Assembler);
                cc = this.MemberCompiler.CodeCompiler.Compile(cc, method.Body);
                context.CodeContexts.Add(cc);
            }

            return base.Compile(context, method);
        }
    }
}
