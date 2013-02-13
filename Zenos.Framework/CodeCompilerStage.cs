using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Zenos.Framework
{
    public abstract class CodeCompilerStage : MemberCompilerStage, ICodeCompiler
    {
        public override void Compile(IMemberContext context, MethodDefinition method)
        {
            var cc = context.GetCompilationContext(method);
            if (cc == null)
                return;

            if(method.HasParameters)
                this.Compile(cc, method.Parameters);

            if(method.HasBody)
                this.Compile(cc, method.Body);
        }

        public virtual void Compile(ICompilationContext context, MethodBody body)
        {
            this.Compile(context, body.ExceptionHandlers);
            this.Compile(context, body.Variables);
            this.Compile(context, body.Scope);
            this.Compile(context, body.Instructions);
        }

        public virtual void Compile(ICompilationContext context, Collection<ParameterDefinition> parameters)
        {
            foreach (var parameter in parameters)
                this.Compile(context, parameter);
        }

        public virtual void Compile(ICompilationContext context, ParameterDefinition parameter)
        {
        }

        public virtual void Compile(ICompilationContext context, Collection<VariableDefinition> variables)
        {
            foreach (var variable in variables)
                this.Compile(context, variable);
        }

        public virtual void Compile(ICompilationContext context, Scope scope)
        {
        }

        public virtual void Compile(ICompilationContext context, VariableDefinition variable)
        {
        }

        public virtual void Compile(ICompilationContext context, Collection<ExceptionHandler> exceptions)
        {
            foreach (var exception in exceptions)
                this.Compile(context, exception);
        }

        public virtual void Compile(ICompilationContext context, ExceptionHandler exception)
        {
        }

        public virtual void Compile(ICompilationContext context, Collection<Instruction> instructions)
        {
            foreach (var instruction in instructions)
                this.Compile(context, instruction);
        }

        public virtual void Compile(ICompilationContext context, Instruction instruction)
        {
        }
    }
}