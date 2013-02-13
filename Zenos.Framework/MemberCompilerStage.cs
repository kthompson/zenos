using Mono.Cecil;

namespace Zenos.Framework
{
    public abstract class MemberCompilerStage : CompilerStage, IMemberCompilerStage
    {
        public override void Compile(ICompilerContext context, ModuleDefinition module)
        {
            foreach (var type in module.Types)
            {
                var mc = context.GetMemberContext(type);
                if(mc == null)
                    continue;
                
                this.Compile(mc, type);
            }
        }

        public virtual void Compile(IMemberContext context, TypeDefinition type)
        {
            foreach (var method in type.Methods)
                this.Compile(context, method);

            foreach (var property in type.Properties)
                this.Compile(context, property);

            foreach (var field in type.Fields)
                this.Compile(context, field);

            foreach (var @event in type.Events)
                this.Compile(context, @event);
        }

        public virtual void Compile(IMemberContext context, EventDefinition @event)
        {
        }

        public virtual void Compile(IMemberContext context, FieldDefinition field)
        {
        }

        public virtual void Compile(IMemberContext context, MethodDefinition method)
        {
        }

        public virtual void Compile(IMemberContext context, PropertyDefinition property)
        {
        }
    }
}