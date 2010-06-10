using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public class CodeCompiler : CodeCompilerStage
    {
        public List<CodeCompilerStage> Stages { get; private set; }

        public CodeCompiler()
        {
            this.Stages = new List<CodeCompilerStage>();
        }

        public override ICodeContext Compile(ICodeContext context)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current));
        }

        public override ICodeContext Compile(ICodeContext context, ExceptionHandler exception)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, exception));
        }

        public override ICodeContext Compile(ICodeContext context, Instruction instruction)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, instruction));
        }

        public override ICodeContext Compile(ICodeContext context, MethodBody body)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, body));
        }

        public override ICodeContext Compile(ICodeContext context, ParameterDefinition parameter)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, parameter));
        }

        public override ICodeContext Compile(ICodeContext context, Scope scope)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, scope));
        }

        public override ICodeContext Compile(ICodeContext context, VariableDefinition variable)
        {
            return this.Stages.Aggregate(context, (current, stage) => stage.Compile(current, variable));
        }
    }
}
