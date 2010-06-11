using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public abstract class CodeCompilerStage : ICodeCompiler
    {
        public CodeCompiler CodeCompiler { get; private set; }

        protected CodeCompilerStage(CodeCompiler compiler)
        {
            this.CodeCompiler = compiler;
        }

        public virtual ICodeContext Compile(ICodeContext context)
        {
            return context;
        }

        public virtual ICodeContext Compile(ICodeContext context, MethodBody body)
        {
            return context;
        }

        public virtual ICodeContext Compile(ICodeContext context, ParameterDefinition parameter)
        {
            return context;
        }

        public virtual ICodeContext Compile(ICodeContext context, Scope scope)
        {
            return context;
        }

        public virtual ICodeContext Compile(ICodeContext context, VariableDefinition variable)
        {
            return context;
        }

        public virtual ICodeContext Compile(ICodeContext context, ExceptionHandler exception)
        {
            return context;
        }

        public virtual ICodeContext Compile(ICodeContext context, Instruction instruction)
        {
            return context;
        }
    }
}