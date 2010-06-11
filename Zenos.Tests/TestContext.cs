using System.IO;
using Mono.Cecil;
using Zenos.Framework;

namespace Zenos.Tests
{
    internal class TestContext : CompilerContext
    {
        public object[] Arguments { get; private set; }

        public TestContext(string outputFile, object[] arguments) 
            : base(outputFile)
        {
            this.Arguments = arguments;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.IsDisposed) 
                return;

            if (disposing && File.Exists(this.OutputFile))
                File.Delete(this.OutputFile);

            base.Dispose(disposing);
        }
    }
}