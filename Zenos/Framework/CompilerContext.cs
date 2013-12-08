using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenos.Framework
{
    public class CompilerContext : ICompilerContext
    {
        readonly Dictionary<object, IMemberContext> _memberContexts = new Dictionary<object, IMemberContext>();

        public bool IsDisposed { get; private set; }
        public string OutputFile { get; private set; }

        public IMemberContext[] Members
        {
            get { return _memberContexts.Values.ToArray(); }
        }

        public CompilerContext(string outputFile)
        {
            this.OutputFile = outputFile;
        }

        public IMemberContext GetMemberContext(object key)
        {
            if (_memberContexts.ContainsKey(key))
                return _memberContexts[key];

            return null;
        }

        public IMemberContext GetOrCreateMemberContext(object key)
        {
            return GetMemberContext(key) ?? CreateMemberContext(key);
        }

        public IMemberContext CreateMemberContext()
        {
            return CreateMemberContext(Guid.NewGuid().ToString());
        }

        public IMemberContext CreateMemberContext(object key)
        {
            return (_memberContexts[key] = new MemberContext(this));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing)
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