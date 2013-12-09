using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class AssemblyContext : IAssemblyContext
    {
        readonly Dictionary<object, ITypeContext> _memberContexts = new Dictionary<object, ITypeContext>();

        public bool IsDisposed { get; private set; }
        public string OutputFile { get; private set; }

        public ITypeContext[] Types
        {
            get { return _memberContexts.Values.ToArray(); }
        }

        public ModuleDefinition Module { get; private set; }

        public AssemblyContext(ModuleDefinition module, string outputFile)
        {
            this.Module = module;
            this.OutputFile = outputFile;
        }

        public ITypeContext GetTypeContext(TypeDefinition type)
        {
            if (_memberContexts.ContainsKey(type))
                return _memberContexts[type];

            return null;
        }

        public ITypeContext GetOrCreateTypeContext(TypeDefinition type)
        {
            return GetTypeContext(type) ?? CreateTypeContext(type);
        }

        public ITypeContext CreateTypeContext(TypeDefinition type)
        {
            return (_memberContexts[type] = new TypeContext(this, type));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing)
            {
                foreach (var member in this.Types)
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