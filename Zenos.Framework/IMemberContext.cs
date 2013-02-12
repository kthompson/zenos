using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface IMemberContext : IDisposable
    {
        ICompilerContext Context { get; }
        bool IsDisposed { get; }
        List<ICompilationContext> CodeContexts { get; }
    }
}