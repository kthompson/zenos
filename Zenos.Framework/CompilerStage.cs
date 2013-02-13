using Mono.Cecil;

namespace Zenos.Framework
{
    public abstract class CompilerStage : ICompiler
    {
        public virtual void Compile(ICompilerContext context, AssemblyDefinition assembly)
        {
            this.Compile(context, assembly.Name);

            foreach (var module in assembly.Modules)
            {
                this.Compile(context, module);
            }
        }

        public virtual void Compile(ICompilerContext context, AssemblyNameDefinition assemblyName)
        {
        }

        public virtual void Compile(ICompilerContext context, ModuleDefinition module)
        {
        }
    }
}