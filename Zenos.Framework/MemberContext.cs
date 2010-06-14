using System;
using System.Collections.Generic;

namespace Zenos.Framework
{
    public class MemberContext : IMemberContext
    {
        public ICompilerContext Context { get; private set; }

        public List<ICodeContext> CodeContexts { get; private set; }
        public bool IsDisposed { get; private set; }

        public MemberContext(ICompilerContext context)
        {
            this.Context = context;
            this.CodeContexts = new List<ICodeContext>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed) 
                return;

            if (disposing)
            {
                foreach (var code in this.CodeContexts)
                    code.Dispose();
            }

            this.IsDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}