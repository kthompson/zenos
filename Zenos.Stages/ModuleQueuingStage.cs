using System.Linq;
using Mono.Cecil;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class ModuleQueuingStage : CompilerStage
    {
        public MemberCompiler MemberCompiler { get; set; }

        public ModuleQueuingStage(MemberCompiler memberCompiler)
        {
            this.MemberCompiler = memberCompiler;
        }

        public override void Compile(ICompilerContext context, AssemblyDefinition assembly)
        {
            if (assembly.Modules.Count == 0)
                return;

            foreach (var module in assembly.Modules)
            {
                this.Compile(context, module);
            }
        }
        
        public override void Compile(ICompilerContext context, ModuleDefinition module)
        {
            foreach (var type in module.Types)
            {
                var member = new MemberContext(context);
                context.Members.Add(member);
                this.MemberCompiler.Compile(member, type);
            }
        }
    }
}