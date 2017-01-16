using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

using CecilInstruction = Mono.Cecil.Cil.Instruction;
using CecilFlowControl = Mono.Cecil.Cil.FlowControl;
using Mono.Collections.Generic;
using Zenos.Framework;

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
            var chain = new ZenosInstruction
            {
                Code = InstructionCode.CilNop
            };

            var cilInstMap = FillInstructions(cecilInstr, chain);

            context.Instruction = chain;

            CleanupOperands(chain, cilInstMap);
            return context;
        }

        private static Dictionary<long, IInstruction> FillInstructions(CecilInstruction cecilInstr, IInstruction chain)
        {
            var instrOffsetMap = new Dictionary<long, IInstruction>();
            while (cecilInstr != null)
            {
                var instruction = CilToZenosInstruction(cecilInstr);
                chain = chain.Add(instruction);
                
                instrOffsetMap[instruction.Offset] = instruction;

                cecilInstr = cecilInstr.Next;
            }
            return instrOffsetMap;
        }

        private static void CleanupOperands(IInstruction chain, Dictionary<long, IInstruction> instrOffsetMap)
        {
            foreach (var inst in chain)
            {
                var cil = inst.Operand0 as CecilInstruction;
                if (cil != null)
                {
                    //switch the cecil instruction out for our IInstruction
                    inst.Operand0 = instrOffsetMap[cil.Offset];
                }
            }
        }

        private static ZenosInstruction CilToZenosInstruction(CecilInstruction cecilInstr)
        {
            return new ZenosInstruction()
            {
                Code = (InstructionCode) cecilInstr.OpCode.Code,
                Offset = cecilInstr.Offset,
                SourceInstruction = cecilInstr,
                Operand0 = cecilInstr.Operand,
                FlowControl = (FlowControl)cecilInstr.OpCode.FlowControl
            };
        }
    }
}
