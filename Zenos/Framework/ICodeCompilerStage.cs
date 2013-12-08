using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Zenos.Framework
{
    public interface ICodeCompilerStage : IMemberCompilerStage
    {
        void Compile(ICompilationContext context, MethodBody body);
        void Compile(ICompilationContext context, Scope scope);

        void Compile(ICompilationContext context, Collection<ParameterDefinition> parameters);
        void Compile(ICompilationContext context, ParameterDefinition parameter);

        void Compile(ICompilationContext context, Collection<VariableDefinition> variables);
        void Compile(ICompilationContext context, VariableDefinition variable);

        void Compile(ICompilationContext context, Collection<ExceptionHandler> exceptions);
        void Compile(ICompilationContext context, ExceptionHandler exception);

        void Compile(ICompilationContext context, Collection<Instruction> instructions);
        void Compile(ICompilationContext context, Instruction instruction);
    }
}
