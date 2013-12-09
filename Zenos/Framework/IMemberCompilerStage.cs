using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public interface IMemberCompilerStage : ICompilerStage
    {
        void Compile(ITypeContext context);
    }
}
