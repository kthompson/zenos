using System;
using System.Collections.Generic;

namespace Zenos.Framework
{
    public class CompilerContext : ICompilerContext
    {
        public List<IMemberContext> Members { get; private set; }
        public bool IsDisposed { get; private set; }

        public string OutputFile { get; private set; }

        public CompilerContext(string outputFile)
        {
            this.OutputFile = outputFile;
            this.Members = new List<IMemberContext>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed) 
                return;

            if(disposing)
            {
                foreach (var member in this.Members)
                    member.Dispose();  
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