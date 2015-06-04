using System;
using Mono.Cecil;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public abstract class Architecture : IArchitecture
    {
        public abstract string Name { get; }
        public void CreateVariables(IMethodContext context)
        {
            throw new NotImplementedException();
        }
    }
}