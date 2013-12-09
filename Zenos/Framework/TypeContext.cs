using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class TypeContext : ITypeContext
    {
        readonly Dictionary<object, IMethodContext> _compilationContexts = new Dictionary<object, IMethodContext>();

        public IAssemblyContext Context { get; private set; }
        public bool IsDisposed { get; private set; }
        public TypeDefinition Type { get; private set; }

        public IMethodContext[] MethodContexts
        {
            get { return _compilationContexts.Values.ToArray(); }
        }

        public TypeContext(IAssemblyContext context, TypeDefinition type)
        {
            this.Context = context;
            this.Type = type;
        }

        public IMethodContext GetMethodContext(MethodDefinition method)
        {
            if (_compilationContexts.ContainsKey(method))
                return _compilationContexts[method];

            return null;
        }

        public IMethodContext GetOrCreateMethodContext(MethodDefinition method)
        {
            return GetMethodContext(method) ?? CreateMethodContext(method);
        }

        public IMethodContext CreateMethodContext(MethodDefinition method)
        {
            return (_compilationContexts[method] = new MethodContext(this, method));
        }
        

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing)
            {
                foreach (var code in this.MethodContexts)
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