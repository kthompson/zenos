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
            IInstruction instruction = null;
            var instrOffsetMap = new Dictionary<long, IInstruction>();
            while (cecilInstr != null)
            {
                instruction = CilToZenosInstruction(cecilInstr);
                instrOffsetMap[instruction.Offset] = instruction;

                if (prev == null)
                {
                    //this must be the first instruction
                    context.Instruction = instruction;
                }
                else
                {
                    prev.SetNext(instruction);
                }

                cecilInstr = cecilInstr.Next;
                prev = instruction;
            }

            //reset to first instruction so we can check operands
            instruction = context.Instruction;

            while (instruction != null)
            {
                var cil = instruction.Operand as Instruction;
                if (cil != null)
                {
                    //switch the cecil instruction out for our IInstruction
                    instruction.Operand = instrOffsetMap[cil.Offset];
                }

                instruction = instruction.Next;
            }

        }

        private static IrInstruction CilToZenosInstruction(Instruction cecilInstr)
        {
            return new IrInstruction
            {
                Code = (InstructionCode) cecilInstr.OpCode.Code,
                Offset = cecilInstr.Offset,
                SourceInstruction = cecilInstr,
                Operand = cecilInstr.Operand
            };
        }
    }

}
