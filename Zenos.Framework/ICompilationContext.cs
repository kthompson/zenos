using System;
using System.IO;

namespace Zenos.Framework
{
    public interface ICompilationContext : IDisposable
    {
        IMemberContext Context { get; }
        bool IsDisposed { get; }

        Sections Sections { get; }
        string OutputFile { get; set; }
        Section Text { get; }
        Section Data { get; }

        string CreateLabel(string prefix = null);
    }
}