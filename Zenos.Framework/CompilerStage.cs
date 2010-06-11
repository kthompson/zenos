using Mono.Cecil;

namespace Zenos.Framework
{
    public abstract class CompilerStage : ICompiler
    {
        public Compiler Compiler { get; protected set; }

        protected CompilerStage(Compiler compiler)
        {
            this.Compiler = compiler;
        }

        public virtual ICompilerContext Compile(ICompilerContext context)
        {
            return context;
        }

        public virtual ICompilerContext Compile(ICompilerContext context, AssemblyDefinition assembly)
        {
            return context;
        }

        public virtual ICompilerContext Compile(ICompilerContext context, AssemblyNameDefinition assemblyName)
        {
            return context;
        }

        public virtual ICompilerContext Compile(ICompilerContext context, ModuleDefinition module)
        {
            return context;
        }
    }
}