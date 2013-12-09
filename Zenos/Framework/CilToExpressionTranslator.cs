using System;
using System.Diagnostics;
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
            context.Sections["drectve"].WriteLine(".section .drectve");

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

            this.Compile(context, body.Instructions);

            //base.Compile(context, body);

            context.Text.WriteLine("# epilogue ");

            //reset to callee stack frame
            context.Text.WriteLine("    leave               # restore calling function stack frame");
            context.Text.WriteLine("    ret");

        }

    }
}