using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zenos.Framework
{
    public class TypeContext : ITypeContext
    {
        readonly Dictionary<string, IMethodContext> _compilationContexts = new Dictionary<string, IMethodContext>();

        public bool IsDisposed { get; private set; }

        public void Add(string key, IMethodContext methodContext)
        {
            _compilationContexts[key] = methodContext;
        }

        public IMethodContext GetMethodContext(string key)
        {
            IMethodContext mc;
            _compilationContexts.TryGetValue(key, out mc);
            return mc;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing)
            {
                foreach (var code in this)
                    code.Dispose();
            }

            this.IsDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<IMethodContext> GetEnumerator()
        {
            return _compilationContexts.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}