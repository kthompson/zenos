﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class EmitterStage : CodeCompilerStage
    {
        public override void Compile(ICompilationContext context, MethodBody body)
        {
            context.OutputFile = body.Method.Name.ToFileName().AppendRandom("_", 32, ".s");

            context.Text.WriteLine(".globl _{0}", body.Method.Name);
            context.Text.WriteLine(".def	_{0};	.scl	2;	.type	32;	.endef", body.Method.Name);
            context.Text.WriteLine("_{0}:", body.Method.Name);

            if (body.Instructions.Count == 0)
                return;

            context.Text.WriteLine("# prologue ");
            //save callee stack frame 
            context.Text.WriteLine("pushl %ebp          # store the stack frame of the calling function on the stack");
            context.Text.WriteLine("movl %esp, %ebp     # takes the current stack pointer and uses it as the frame for the called function");

            //add variable space to stack
            if (body.HasVariables)
                context.Text.WriteLine("subl ${0}, %esp     # make room for local variables ", body.Variables.Count * 4);

            context.Text.WriteLine("# body ");

            base.Compile(context, body);

            context.Text.WriteLine("# epilogue ");

            //reset to callee stack frame
            context.Text.WriteLine("leave               # restore calling function stack frame");
            context.Text.WriteLine("ret");
        }

        public override void Compile(ICompilationContext context, Instruction instruction)
        {
            Trace.WriteLine(instruction);

            switch (instruction.OpCode.Code)
            {
                case Code.Ldc_I4:
                    context.Text.WriteLine("movl ${0}, %eax     # {1}", instruction.Operand, instruction);
                    break;
                case Code.Stloc:
                    context.Text.WriteLine("movl %eax, {0}      # {1} ", EmitLocation(context, instruction), instruction);
                    break;
                case Code.Nop:
                    break;
                case Code.Ldloc:
                    var varType = ((VariableReference) (instruction.Operand)).VariableType;
                    var inst = varType.FullName == "System.Single" ? "flds {0}       # {1} " : "movl {0}, %eax       # {1} ";

                    context.Text.WriteLine(inst, EmitLocation(context, instruction), instruction);
                    break;
                case Code.Ldarg:
                    context.Text.WriteLine("movl {0}, %eax      # {1} ", EmitLocation(context, instruction), instruction);
                    break;
                case Code.Ret:
                    //ret is handled in the method body
                    Helper.IsNull(instruction.Next);
                    break;
                case Code.Ldc_R4:
                    var inIEEE754 = BitConverter.ToInt32(BitConverter.GetBytes((float) instruction.Operand), 0);
                    context.Text.WriteLine("movl $0x{0}, %eax    # {1}", inIEEE754.ToString("x"),  instruction);
                    break;
                default:
                    Helper.NotSupported(string.Format("Instruction not supported: {0}", instruction));
                    break;
            }
        }

        private string EmitLocation(ICompilationContext context, Instruction instruction)
        {
            if (instruction.Operand is VariableReference)
            {
                var v = instruction.Operand as VariableReference;
                var index = (v.Index + 1) * -4;
                return string.Format("{0}(%ebp)", index);
            }

            if (instruction.Operand is ParameterReference)
            {
                var v = instruction.Operand as ParameterReference;
                var index = 8 + (v.Index) * 4;
                return string.Format("{0}(%ebp)", index);
            }
            Helper.Break();
            return instruction.Operand.ToString();
        }
    }
}