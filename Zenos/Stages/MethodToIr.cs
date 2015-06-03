using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class MethodToIr : CodeCompilerStage
    {
        private readonly IArchitecture _architecture;

        public MethodToIr(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        public override void Compile(IMethodContext context)
        {
            
        }
    }
}