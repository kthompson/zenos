using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Zenos.Framework
{
    public interface ICodeCompilerStage : IMemberCompilerStage
    {
        void Compile(IMethodContext context);
    }
}
