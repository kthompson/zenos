using Mono.Cecil;

namespace Zenos.Framework
{
    public abstract class CompilerStage : ICompilerStage
    {
        public abstract void Compile(IAssemblyContext context);
    }
}