using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.FSharp.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CecilInstruction = Mono.Cecil.Cil.Instruction;
using CecilFlowControl = Mono.Cecil.Cil.FlowControl;
using Mono.Collections.Generic;
using Zenos.Framework;
using FlowControl = Zenos.Framework.FlowControl;
using Instruction = Zenos.Framework.Instruction;

namespace Zenos.Stages
{
    public class CecilToZenos : Compiler<IMethodContext>
    {
        public override IMethodContext Compile(IMethodContext context)
        {
            
            var cecilInstr = context.Method.Body.Instructions.FirstOrDefault();

            return Compile(context, cecilInstr);
        }

        private IMethodContext Compile(IMethodContext context, CecilInstruction cecilInstr)
        {
            var chain = new Instruction(
                InstructionCode.NewCil(Code.Nop),
                FSharpOption<int>.None,
                FSharpOption<Operand>.None,
                FSharpOption<Operand>.None,
                FSharpOption<Operand>.None,
                FSharpOption<Operand>.None,
                FSharpOption<CecilInstruction>.None
            );

            var cilInstMap = FillInstructions(cecilInstr, chain);

            context.Instruction = chain;

            CleanupOperands(chain, cilInstMap);
            return context;
        }

        private static Dictionary<long, Instruction> FillInstructions(CecilInstruction cecilInstr, Instruction chain)
        {
            var instrOffsetMap = new Dictionary<long, Instruction>();
            //while (cecilInstr != null)
            //{
            //    var instruction = CilToZenosInstruction(cecilInstr);
            //    chain = chain.Add(instruction);

            //    instrOffsetMap[instruction.Offset] = instruction;

            //    cecilInstr = cecilInstr.Next;
            //}
            return instrOffsetMap;
        }

        private static void CleanupOperands(Instruction chain, Dictionary<long, Instruction> instrOffsetMap)
        {
            //foreach (var inst in chain)
            //{
            //    var cil = inst.Operand0 as InstructionOperand;
            //    if (cil != null)
            //    {
            //        //switch the cecil instruction out for our IInstruction
            //        inst.Operand0 = Operand.Instruction(instrOffsetMap[cil.Instruction.Offset]);
            //    }
            //}
        }

        private static Instruction CilToZenosInstruction(CecilInstruction cecilInstr)
        {
            return new Instruction(
                InstructionCode.NewCil(cecilInstr.OpCode.Code),
                FSharpOption<int>.Some(cecilInstr.Offset),
                FSharpOption<Operand>.None,
                FSharpOption<Operand>.None,
                FSharpOption<Operand>.None,

                FSharpOption<Operand>.None,

                FSharpOption<CecilInstruction>.Some(cecilInstr)
            );
            //{
            //    Operand0 = cecilInstr.Operand,
            //    FlowControl = (FlowControl)cecilInstr.OpCode.FlowControl
            //};
        }
    }
}
