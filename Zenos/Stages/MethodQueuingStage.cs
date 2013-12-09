using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mono.Cecil;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class MethodQueuingStage : MemberCompilerStage
    {
        public override void Compile(ITypeContext context)
        {
            foreach (var method in context.Type.Methods)
                context.CreateMethodContext(method);
        }
    }
}
