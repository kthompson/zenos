using System.Linq;
using Mono.Cecil;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class ModuleQueuingStage : CompilerStage
    {
        public ModuleQueuingStage(Compiler compiler)
            : base(compiler)
        {
        }

        public override ICompilerContext Compile(ICompilerContext context, AssemblyDefinition assembly)
        {
            if (assembly.Modules.Count == 0)
                return base.Compile(context, assembly);

            return assembly.Modules.Aggregate(context, (current, module) => this.Compiler.Compile(current, module));
        }

        public override ICompilerContext Compile(ICompilerContext context, ModuleDefinition module)
        {
            foreach (var type in module.Types)
            {
                var mc = this.Compiler.MemberCompiler.Compile(new MemberContext(context), type);
                context.Members.Add(mc);
            }
            
            return base.Compile(context, module);
        }
    }
}