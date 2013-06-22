using System;
using System.Collections.Generic;
using System.IO;

namespace Zenos.Framework
{
    public class CompilationContext : ICompilationContext
    {
        public IMemberContext Context { get; private set; }
        public StringWriter Text { get; private set; }
        public StringWriter Data { get; private set; }
        public string OutputFile { get; set; }
        public bool IsDisposed { get; private set; }

        public CompilationContext(IMemberContext context)
        {
            this.Context = context;
            this.Text = new StringWriter();
            this.Data = new StringWriter();
        }

        private int _lastLabel = 1;
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

            if (this.Text != null)
            {
                this.Text.Dispose();
                this.Text = null;
            }

            if (this.Data != null)
            {
                this.Data.Dispose();
                this.Data = null;
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