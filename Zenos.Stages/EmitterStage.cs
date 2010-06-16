using System;
using System.Collections.Generic;
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
        public EmitterStage(CodeCompiler compiler)
            : base(compiler)
        {
        }

        public override ICodeContext Compile(ICodeContext context, MethodBody body)
        {
            context.Text.WriteLine(".globl _{0}", body.Method.Name);
            context.Text.WriteLine(".def	_{0};	.scl	2;	.type	32;	.endef", body.Method.Name);
            context.Text.WriteLine("_{0}:", body.Method.Name);

            if (body.Instructions.Count == 0)
                return base.Compile(context, body);

            //save callee stack frame 
            context.Text.WriteLine("pushl %ebp");
            context.Text.WriteLine("movl %esp, %ebp");

            //add variable space to stack
            if (body.HasVariables)
                context.Text.WriteLine("subl ${0}, %esp", body.Variables.Count * 4);

            context = this.Compile(context, body.Instructions);

            //reset to callee stack frame
            context.Text.WriteLine("leave");
            context.Text.WriteLine("ret");

            return context;
        }

        public override ICodeContext Compile(ICodeContext context, Collection<VariableDefinition> variables)
        {
            return base.Compile(context, variables);
        }

        public override ICodeContext Compile(ICodeContext context, Collection<Instruction> instructions)
        {
            return instructions.Aggregate(context, this.Compile);
        }

        public override ICodeContext Compile(ICodeContext context, Instruction instruction)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Ldc_I4:
                    context.Text.WriteLine("movl ${0}, %eax", instruction.Operand);
                    break;
                case Code.Stloc:
                    context.Text.WriteLine(EmitStoreInstruction(context, instruction));
                    break;
                case Code.Nop:
                    break;
                case Code.Ldloc:
                    context.Text.WriteLine(EmitLoadInstruction(context, instruction));
                    break;
                case Code.Ldarg:
                    context.Text.WriteLine(EmitLoadInstruction(context, instruction));
                    break;
                case Code.Ret:
                    //ret is handled in the method body
                    Helper.IsNull(instruction.Next);
                    break;
                default:
                    Helper.NotSupported(string.Format("Instruction not supported: {0}", instruction));
                    break;
            }

            return context;
        }

        private string EmitLoadInstruction(ICodeContext context, Instruction instruction)
        {
            return string.Format("movl {0}, %eax", EmitLocation(context, instruction));
        }

        private string EmitStoreInstruction(ICodeContext context, Instruction instruction)
        {
            return string.Format("movl %eax, {0}", EmitLocation(context, instruction));
        }

        private string EmitLocation(ICodeContext context, Instruction instruction)
        {
            if (instruction.Operand is VariableReference)
            {
                var v = instruction.Operand as VariableReference;
                var index = v.Index * -4;
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
