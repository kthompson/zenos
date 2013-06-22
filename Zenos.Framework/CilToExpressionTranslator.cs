using System;
using System.Diagnostics;
using Cecil.Decompiler;
using Cecil.Decompiler.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Zenos.Core;

namespace Zenos.Framework
{
    public class CilToExpressionTranslator : CodeCompilerStage
    {

        public override void Compile(ICompilationContext context, MethodBody body)
        {
            context.OutputFile = body.Method.Name.ToFileName().AppendRandom("_", 32, ".s");

            //setup data section
            context.Data.WriteLine(".section .rdata,\"dr\"");

            context.Text.WriteLine(".globl _{0}", body.Method.Name);
            context.Text.WriteLine(".def	_{0};	.scl	2;	.type	32;	.endef", body.Method.Name);
            context.Text.WriteLine("_{0}:", body.Method.Name);

            if (body.Instructions.Count == 0)
                return;

            context.Text.WriteLine("# prologue ");
            //save callee stack frame 
            context.Text.WriteLine("    pushl %ebp          # store the stack frame of the calling function on the stack");
            context.Text.WriteLine("    movl %esp, %ebp     # takes the current stack pointer and uses it as the frame for the called function");

            //add variable space to stack
            if (body.HasVariables)
                context.Text.WriteLine("    subl ${0}, %esp     # make room for local variables ", body.Variables.Count * 4);

            context.Text.WriteLine("# body ");

            this.Compile(context, body.Decompile());

            //base.Compile(context, body);

            context.Text.WriteLine("# epilogue ");

            //reset to callee stack frame
            context.Text.WriteLine("    leave               # restore calling function stack frame");
            context.Text.WriteLine("    ret");

        }

        public virtual void Compile(ICompilationContext context, BlockStatement block)
        {
            foreach (var statement in block.Statements)
                this.Compile(context, statement);
        }

        public virtual void Compile(ICompilationContext context, Statement statement)
        {
            switch (statement.CodeNodeType)
            {
                case CodeNodeType.ExpressionStatement:
                    Compile(context, (ExpressionStatement)statement);
                    break;
                case CodeNodeType.ReturnStatement:
                    Compile(context, (ReturnStatement)statement);
                    break;
                default:
                    Helper.Break();
                    break;
            }
        }

        public virtual void Compile(ICompilationContext context, ExpressionStatement statement)
        {
            Compile(context, statement.Expression);
        }

        public virtual void Compile(ICompilationContext context, ReturnStatement statement)
        {
        }


        public virtual void Compile(ICompilationContext context, Expression expression)
        {
            switch (expression.CodeNodeType)
            {
                case CodeNodeType.AssignExpression:
                    Compile(context, (AssignExpression)expression);
                    break;

                case CodeNodeType.LiteralExpression:
                    Compile(context, (LiteralExpression)expression);
                    break;

                default:
                    Helper.Break();
                    break;
            }
        }

        public virtual void Compile(ICompilationContext context, AssignExpression expression)
        {
            Load(context, expression.Expression);
            
            Store(context, expression.Target);
        }

        private void Load(ICompilationContext context, Expression expression)
        {
            switch (expression.CodeNodeType)
            {
                case CodeNodeType.LiteralExpression:
                    EmitLiteral(context, ((LiteralExpression) expression).Value);
                    break;

                case CodeNodeType.VariableReferenceExpression:
                    var v = ((VariableReferenceExpression) expression).Variable;
                    var location = GetVariableLocation(v);

                    context.Text.WriteLine("    movl {0}, %eax       # [{1}] {2} ", location, v.Index, v.VariableType);
                    break;

                default:
                    Helper.Break();
                    break;
            }
        }

        private void Store(ICompilationContext context, Expression target)
        {
            if (target is VariableReferenceExpression)
            {
                var v = ((VariableReferenceExpression)target).Variable;
                var location = GetVariableLocation(v);

                context.Text.WriteLine("    movl %eax, {0}      # [{1}] {2} ", location, v.Index, v.VariableType);
            }
            else
            {
                Helper.Stop();
            }

        }

        public virtual void Compile(ICompilationContext context, LiteralExpression expression)
        {
            //TODO: STORE IN 
            EmitLiteral(context, expression.Value);
        }

        public virtual void Compile(ICompilationContext context, VariableReferenceExpression expression)
        {
            var v = expression.Variable;

            var location = GetVariableLocation(v);

            context.Text.WriteLine("    movl {0}, %eax       # [{1}] {2} ", location, v.Index, v.VariableType);
        }

        private static string GetVariableLocation(VariableReference v)
        {
            var index = (v.Index + 1)*-4;
            var location = string.Format("{0}(%ebp)", index);
            return location;
        }

        private void EmitLiteral(ICompilationContext context, object value)
        {
            if (value is int)
            {
                context.Text.WriteLine("    movl ${0}, %eax     # {1}", (int)value, value.GetType());
            }
            else if (value is long)
            {
                var l = (long) value;
                var bytes = BitConverter.GetBytes(l);
                var low = BitConverter.ToInt32(bytes, 0);
                var hi = BitConverter.ToInt32(bytes, 4);
                context.Text.WriteLine("    movl ${0}, %eax     # low {1}", low, l.ToString("X"));
                context.Text.WriteLine("    movl ${0}, %edx     # high {1}", hi, l.ToString("X"));
            }
            else if (value is bool)
            {
                context.Text.WriteLine("    movl ${0}, %eax     # {1}", (bool)value ?  0 : 1, value);
            }
            else if (value is float)
            {
                var label = context.CreateLabel("LC");
                context.Data.WriteLine("    .align 4");
                context.Data.WriteLine(label + ":");
                context.Data.WriteLine("    .long " + SingleToImmediate((float)value));

                context.Text.WriteLine("    flds {0}     # {1}", label, value);
            }
            else if (value is double)
            {
                var d = (double) value;
                var bytes = BitConverter.GetBytes(d);

                var low = BitConverter.ToInt32(bytes, 0);
                var hi = BitConverter.ToInt32(bytes, 4);

                var label = context.CreateLabel("LC");

                context.Data.WriteLine("    .align 8");
                context.Data.WriteLine(label + ":");
                context.Data.WriteLine("    .long " + low);
                context.Data.WriteLine("    .long " + hi);

                context.Text.WriteLine("    fldl {0}     # {1}", label, value);
            }
            else
            {
                Helper.Stop();
            }
        }

        private static int SingleToImmediate(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}