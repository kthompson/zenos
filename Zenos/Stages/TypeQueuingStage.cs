using System.Linq;
using Mono.Cecil;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class TypeQueuingStage : CompilerStage
    {
        public override void Compile(IAssemblyContext context)
        {
            foreach (var type in context.Module.Types)
                context.CreateTypeContext(type);
        }
    }
}