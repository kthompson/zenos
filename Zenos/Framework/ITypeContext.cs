using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface ITypeContext : IDisposable, IEnumerable<IMethodContext>
    {
        bool IsDisposed { get; }

        void Add(string key, IMethodContext methodContext);

        IMethodContext GetMethodContext(string key);
    }
}