using System;
using System.IO;

namespace Zenos.Framework
{
    public interface ICompilationContext : IDisposable
    {
        IMemberContext Context { get; }
        bool IsDisposed { get; }
        StringWriter Text { get; }
        CodeType CodeType { get; }
        string OutputFile { get; set; }
    }
}