using Mono.Cecil;

namespace Zenos.Framework
{
    public abstract class MemberCompilerStage : IMemberCompiler
    {
        public virtual IMemberContext Compile(IMemberContext context)
        {
            return context;
        }

        public virtual IMemberContext Compile(IMemberContext context, IMemberDefinition member)
        {
            return context;
        }

        public virtual IMemberContext Compile(IMemberContext context, EventDefinition @event)
        {
            return context;
        }

        public virtual IMemberContext Compile(IMemberContext context, FieldDefinition field)
        {
            return context;
        }

        public virtual IMemberContext Compile(IMemberContext context, MethodDefinition method)
        {
            return context;
        }

        public virtual IMemberContext Compile(IMemberContext context, PropertyDefinition property)
        {
            return context;
        }

        public virtual IMemberContext Compile(IMemberContext context, TypeDefinition type)
        {
            return context;
        }
    }
}