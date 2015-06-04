using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface IAssemblyContext : IDisposable, IEnumerable<ITypeContext>
    {
        bool IsDisposed { get; }

        void Add(string key, ITypeContext type);

        ITypeContext GetTypeContext(string key);
    }
}