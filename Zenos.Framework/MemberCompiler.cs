using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Zenos.Framework
{
    public class MemberCompiler : MemberCompilerStage
    {
        public CodeCompiler CodeCompiler { get; private set; }
        public List<MemberCompilerStage> Stages { get; private set; }

        public MemberCompiler(CodeCompiler cc)
            : base(null)
        {
            this.CodeCompiler = cc;
            this.Stages = new List<MemberCompilerStage>();
        }

        public override IMemberContext Compile(IMemberContext context, EventDefinition @event)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, @event));
        }

        public override IMemberContext Compile(IMemberContext context, FieldDefinition field)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, field));
        }

        public override IMemberContext Compile(IMemberContext context, IMemberDefinition member)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, member));
        }

        public override IMemberContext Compile(IMemberContext context, MethodDefinition method)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, method));
        }

        public override IMemberContext Compile(IMemberContext context, PropertyDefinition property)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, property));
        }

        public override IMemberContext Compile(IMemberContext context, TypeDefinition type)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, type));
        }
    }
}
