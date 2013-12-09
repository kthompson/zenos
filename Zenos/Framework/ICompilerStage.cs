using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface ICompilerStage
    {
        void Compile(IAssemblyContext context);
    }
}
