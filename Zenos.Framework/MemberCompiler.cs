using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class MemberCompiler : MemberCompilerStage
    {
        public List<MemberCompilerStage> Stages { get; private set; }

        public MemberCompiler(IEnumerable<MemberCompilerStage> stages)
        {
            this.Stages = new List<MemberCompilerStage>(stages);
        }

        public override void Compile(IMemberContext context, EventDefinition @event)
        {
            this.Stages.ForEach(stage => stage.Compile(context, @event));
        }

        public override void Compile(IMemberContext context, FieldDefinition field)
        {
            this.Stages.ForEach(stage => stage.Compile(context, field));
        }

        public override void Compile(IMemberContext context, MethodDefinition method)
        {
            this.Stages.ForEach(stage => stage.Compile(context, method));
        }

        public override void Compile(IMemberContext context, PropertyDefinition property)
        {
            this.Stages.ForEach(stage => stage.Compile(context, property));
        }

        public override void Compile(IMemberContext context, TypeDefinition type)
        {
            this.Stages.ForEach(stage => stage.Compile(context, type));
        }
    }
}
