using System;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public abstract class Architecture : IArchitecture
    {
        public abstract string Name { get; }

        public virtual void CreateVariables(IMethodContext context)
        {
        }
    }
}