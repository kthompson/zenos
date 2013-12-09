using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface ITypeContext : IDisposable
    {
        IAssemblyContext Context { get; }
        bool IsDisposed { get; }
        TypeDefinition Type { get; }

        IMethodContext[] MethodContexts { get; }

        IMethodContext GetMethodContext(MethodDefinition method);
        IMethodContext GetOrCreateMethodContext(MethodDefinition method);
        IMethodContext CreateMethodContext(MethodDefinition method);
    }
}