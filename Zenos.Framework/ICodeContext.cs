using System;
using System.IO;

namespace Zenos.Framework
{
    public interface ICodeContext : IDisposable
    {
        IMemberContext Context { get; }
        bool IsDisposed { get; }
        StringWriter Text { get; }
    }
}