using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Zenos.Framework
{
    public interface IMemberContext : IDisposable
    {
        ICompilerContext Context { get; }
        bool IsDisposed { get; }
        ICompilationContext[] CodeContexts { get; }

        ICompilationContext GetCompilationContext(object key);
        ICompilationContext GetOrCreateCompilationContext(object key);
        ICompilationContext CreateCompilationContext(object key);
        ICompilationContext CreateCompilationContext();
    }
}