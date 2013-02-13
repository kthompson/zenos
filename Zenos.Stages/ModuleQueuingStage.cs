using System.Linq;
using Mono.Cecil;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class ModuleQueuingStage : CompilerStage
    {
        public override void Compile(ICompilerContext context, ModuleDefinition module)
        {
            foreach (var type in module.Types)
            {
                context.CreateMemberContext(type);
            }
        }
    }
}