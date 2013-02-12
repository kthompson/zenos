using Mono.Cecil;

namespace Zenos.Framework
{
    public abstract class CompilerStage : ICompiler
    {
        public virtual void Compile(ICompilerContext context, AssemblyDefinition assembly)
        {
        }

        public virtual void Compile(ICompilerContext context, AssemblyNameDefinition assemblyName)
        {
        }

        public virtual void Compile(ICompilerContext context, ModuleDefinition module)
        {
        }
    }
}