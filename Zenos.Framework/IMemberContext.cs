using System;
using System.Collections.Generic;

namespace Zenos.Framework
{
    public interface IMemberContext : IDisposable
    {
        ICompilerContext Context { get; }
        bool IsDisposed { get; }
        List<ICodeContext> CodeContexts { get; }
    }
}