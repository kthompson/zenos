using System;
using System.IO;

namespace Zenos.Framework
{
    public class CodeContext : ICodeContext
    {
        public IMemberContext Context { get; private set; }
        public StringWriter Text { get; private set; }
        public bool IsDisposed { get; private set; }

        public CodeContext(IMemberContext context)
        {
            this.Context = context;
            this.Text = new StringWriter();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing && this.Text != null)
                this.Text.Dispose();

            this.Text = null;
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