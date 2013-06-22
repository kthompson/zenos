using System;
using System.IO;

namespace Zenos.Framework
{
    public interface ICompilationContext : IDisposable
    {
        IMemberContext Context { get; }
        bool IsDisposed { get; }
        StringWriter Text { get; }
        StringWriter Data { get; }
        string OutputFile { get; set; }

        string CreateLabel(string prefix = null);
    }
}