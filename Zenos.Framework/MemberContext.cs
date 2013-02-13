using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class MemberContext : IMemberContext
    {
        readonly Dictionary<object, ICompilationContext> _compilationContexts = new Dictionary<object, ICompilationContext>();

        public ICompilerContext Context { get; private set; }
        public bool IsDisposed { get; private set; }

        public ICompilationContext[] CodeContexts
        {
            get { return _compilationContexts.Values.ToArray(); }
        }

        public MemberContext(ICompilerContext context)
        {
            this.Context = context;
        }

        public ICompilationContext GetCompilationContext(object key)
        {
            if (_compilationContexts.ContainsKey(key))
                return _compilationContexts[key];

            return null;
        }

        public ICompilationContext GetOrCreateCompilationContext(object key)
        {
            return GetCompilationContext(key) ?? CreateCompilationContext(key);
        }

        public ICompilationContext CreateCompilationContext()
        {
            return CreateCompilationContext(Guid.NewGuid().ToString());
        }

        public ICompilationContext CreateCompilationContext(object key)
        {
            return (_compilationContexts[key] = new CompilationContext(this));
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