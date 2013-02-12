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

        public CodeCompiler(IEnumerable<CodeCompilerStage> stages)
        {
            this.Stages = new List<CodeCompilerStage>(stages);
        }

        public override void Compile(ICompilationContext context, ExceptionHandler exception)
        {
            this.Stages.ForEach(stage => stage.Compile(context, exception));
        }

        public override void Compile(ICompilationContext context, Instruction instruction)
        {
            this.Stages.ForEach(stage => stage.Compile(context, instruction));
        }

        public override void Compile(ICompilationContext context, MethodBody body)
        {
            this.Stages.ForEach(stage => stage.Compile(context, body));
        }

        public override void Compile(ICompilationContext context, ParameterDefinition parameter)
        {
            this.Stages.ForEach(stage => stage.Compile(context, parameter));
        }

        public override void Compile(ICompilationContext context, Scope scope)
        {
            this.Stages.ForEach(stage => stage.Compile(context, scope));
        }

        public override void Compile(ICompilationContext context, VariableDefinition variable)
        {
            this.Stages.ForEach(stage => stage.Compile(context, variable));
        }
    }
}
