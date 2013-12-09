using System.IO;
using Mono.Cecil;
using Zenos.Framework;

namespace Zenos.Tests
{
    internal class TestContext : AssemblyContext
    {
        public object[] Arguments { get; private set; }

        public TestContext(ModuleDefinition module, string outputFile, object[] arguments)
            : base(module, outputFile)
        {
            this.Arguments = arguments;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            //if (disposing && File.Exists(this.OutputFile))
            //    File.Delete(this.OutputFile);

            base.Dispose(disposing);
        }
    }
}