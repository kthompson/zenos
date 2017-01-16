using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class AssemblyContext : IAssemblyContext
    {
        private readonly Dictionary<object, ITypeContext> _memberContexts = new Dictionary<object, ITypeContext>();

        public void Add(string key, ITypeContext type)
        {
            _memberContexts[key] = type;
        }

        public ITypeContext GetTypeContext(string key)
        {
            if (_memberContexts.ContainsKey(key))
                return _memberContexts[key];

            return null;
        }

        public IEnumerator<ITypeContext> GetEnumerator()
        {
            return _memberContexts.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}