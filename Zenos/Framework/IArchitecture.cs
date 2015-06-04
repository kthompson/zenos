using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenos.Stages;

namespace Zenos.Framework
{
    public interface IArchitecture
    {
        string Name { get; }

        void CreateVariables(IMethodContext context);
    }
}
