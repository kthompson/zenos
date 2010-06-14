﻿using System;
using System.IO;

namespace Zenos.Framework
{
    public class CodeContext : ICodeContext
    {
        public IMemberContext Context { get; private set; }
        public StringWriter Text { get; private set; }
        public CodeType CodeType { get; private set; }
        public string OutputFile { get; set; }
        public bool IsDisposed { get; private set; }

        public CodeContext(IMemberContext context, CodeType type)
        {
            this.Context = context;
            this.CodeType = type;
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