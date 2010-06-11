using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public interface ICodeCompiler
    {
        ICodeContext Compile(ICodeContext context);

        ICodeContext Compile(ICodeContext context, MethodBody body);
        ICodeContext Compile(ICodeContext context, ParameterDefinition parameter);
        ICodeContext Compile(ICodeContext context, Scope scope);
        ICodeContext Compile(ICodeContext context, VariableDefinition variable);
        ICodeContext Compile(ICodeContext context, ExceptionHandler exception);
        ICodeContext Compile(ICodeContext context, Instruction instruction);
    }
}
