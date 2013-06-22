using System;
using System.Collections.Generic;
using System.IO;

namespace Zenos.Framework
{
    public class CompilationContext : ICompilationContext
    {
        public IMemberContext Context { get; private set; }
        public Sections Sections { get; private set; }
        public string OutputFile { get; set; }
        public bool IsDisposed { get; private set; }

        public CompilationContext(IMemberContext context)
        {
            this.Context = context;
            this.Sections = new Sections();
        }

        private int _lastLabel = 1;
        public Section Text
        {
            get { return this.Sections["text"]; }
        }

        public Section Data
        {
            get { return this.Sections["rdata"]; }
        }

        public string CreateLabel(string prefix = null)
        {
            return (prefix ?? "L") + (_lastLabel++).ToString("D4");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (!disposing)
                return;

            foreach (var section in this.Sections)
            {
                section.Dispose();
            }

            this.Context = null;
            this.IsDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}