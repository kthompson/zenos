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
        public override void Compile(IMemberContext context, MethodDefinition method)
        {
            base.Compile(context, method);

            if (!method.HasBody) 
                return;

            context.CreateCompilationContext(method);
        }
    }
}
