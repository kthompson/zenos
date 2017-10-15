using System;
using System.Collections;
using System.Collections.Generic;

namespace Zenos.Framework
{
    public interface IAssemblyContext : IEnumerable<ITypeContext>
    {
        void Add(string key, ITypeContext type);

        ITypeContext GetTypeContext(string key);
    }
}