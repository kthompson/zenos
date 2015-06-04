using Mono.Cecil;

namespace Zenos.Framework
{
    public abstract class MemberCompilerStage : CompilerStage, IMemberCompilerStage
    {
        public override void Compile(IAssemblyContext context)
        {
            foreach (var mc in context)
                this.Compile(mc);
        }

        public abstract void Compile(ITypeContext context);
    }
}