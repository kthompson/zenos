using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class CecilToZenos : CodeCompilerStage
    {
        public override void Compile(IMethodContext context)
        {
            var cecilInstr = context.Method.Body.Instructions.FirstOrDefault();

            Compile(context, cecilInstr);
        }

        private void Compile(IMethodContext context, Instruction cecilInstr, IInstruction prev = null)
        {
            var chain = new InstructionChain();
            var cilInstMap = FillInstructions(cecilInstr, chain);

            context.Instruction = chain.FirstInstruction;

            CleanupOperands(chain, cilInstMap);
        }

        private static Dictionary<long, IInstruction> FillInstructions(Instruction cecilInstr, InstructionChain chain)
        {
            var instrOffsetMap = new Dictionary<long, IInstruction>();
            while (cecilInstr != null)
            {
                var instruction = CilToZenosInstruction(cecilInstr);
                chain.AssignAndIncrement(instruction);

                instrOffsetMap[instruction.Offset] = instruction;

                cecilInstr = cecilInstr.Next;
            }
            chain.Reset();
            return instrOffsetMap;
        }

        private static void CleanupOperands(InstructionChain chain, Dictionary<long, IInstruction> instrOffsetMap)
        {
            while (!chain.EndOfInstructions)
            {
                var inst = chain.Instruction;
                var cil = inst.Operand0 as Instruction;
                if (cil != null)
                {
                    //switch the cecil instruction out for our IInstruction
                    inst.Operand0 = instrOffsetMap[cil.Offset];
                }

                chain++;
            }
        }

        private static ZenosInstruction CilToZenosInstruction(Instruction cecilInstr)
        {
            return new ZenosInstruction
            {
                Code = (InstructionCode) cecilInstr.OpCode.Code,
                Offset = cecilInstr.Offset,
                SourceInstruction = cecilInstr,
                Operand0 = cecilInstr.Operand
            };
        }
    }
}
