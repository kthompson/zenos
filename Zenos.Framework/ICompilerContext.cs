using System;
using System.Collections;
using System.Collections.Generic;

namespace Zenos.Framework
{
    public interface ICompilerContext : IDisposable
    {
        List<IMemberContext> Members { get; }
        bool IsDisposed { get; }
        string OutputFile { get; }
    }
}