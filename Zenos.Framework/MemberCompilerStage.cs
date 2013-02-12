using Mono.Cecil;

namespace Zenos.Framework
{
    public abstract class MemberCompilerStage : IMemberCompiler
    {
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
    }
}