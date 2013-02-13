using System;
using System.Collections;
using System.Collections.Generic;

namespace Zenos.Framework
{
    public interface ICompilerContext : IDisposable
    {
        bool IsDisposed { get; }
        string OutputFile { get; }

        IMemberContext[] Members { get; }

        IMemberContext GetMemberContext(object key);
        IMemberContext GetOrCreateMemberContext(object key);
        IMemberContext CreateMemberContext(object key);
        IMemberContext CreateMemberContext();
    }
}