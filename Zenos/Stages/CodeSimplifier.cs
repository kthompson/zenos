using System.ComponentModel;
using System.Linq;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public class CilCodeSimplifier : CodeCompilerStage
    {
        public virtual bool Compile(IMethodContext context, InstructionChain chain)
        {
            var ins = chain.Instruction;

            if (ins.SourceInstruction.OpCode.OpCodeType != OpCodeType.Macro)
                return true;

            switch (ins.Code)
            {
                case InstructionCode.CilLdarg_0:
                case InstructionCode.CilLdarg_1:
                case InstructionCode.CilLdarg_2:
                case InstructionCode.CilLdarg_3:
                    Simplify(ins, InstructionCode.CilLdarg, context.Parameters[ins.Code - InstructionCode.CilLdarg_0]);
                    break;

                case InstructionCode.CilLdloc_0:
                case InstructionCode.CilLdloc_1:
                case InstructionCode.CilLdloc_2:
                case InstructionCode.CilLdloc_3:
                    Simplify(ins, InstructionCode.CilLdloc, context.Locals[ins.Code - InstructionCode.CilLdloc_0]);
                    break;

                case InstructionCode.CilStloc_0:
                case InstructionCode.CilStloc_1:
                case InstructionCode.CilStloc_2:
                case InstructionCode.CilStloc_3:
                    Simplify(ins, InstructionCode.CilStloc, context.Locals[ins.Code - InstructionCode.CilStloc_0]);
                    break;

                case InstructionCode.CilLdarg_S:
                    SimplifyFromByte(ins, InstructionCode.CilLdarg);
                    break;
                case InstructionCode.CilLdarga_S:
                    SimplifyFromByte(ins, InstructionCode.CilLdarga);
                    break;
                case InstructionCode.CilStarg_S:
                    SimplifyFromByte(ins, InstructionCode.CilStarg);
                    break;
                case InstructionCode.CilLdloc_S:
                    SimplifyFromByte(ins, InstructionCode.CilLdloc);
                    break;
                case InstructionCode.CilLdloca_S:
                    Simplify(ins, InstructionCode.CilLdloca, context.Variables[(sbyte)ins.Operand0]);
                    break;
                case InstructionCode.CilStloc_S:
                    SimplifyFromSByte(ins, InstructionCode.CilStloc);
                    break;
                case InstructionCode.CilLdc_I4_M1:
                case InstructionCode.CilLdc_I4_0:
                case InstructionCode.CilLdc_I4_1:
                case InstructionCode.CilLdc_I4_2:
                case InstructionCode.CilLdc_I4_3:
                case InstructionCode.CilLdc_I4_4:
                case InstructionCode.CilLdc_I4_5:
                case InstructionCode.CilLdc_I4_6:
                case InstructionCode.CilLdc_I4_7:
                case InstructionCode.CilLdc_I4_8:
                    Simplify(ins, InstructionCode.CilLdc_I4, ins.Code - InstructionCode.CilLdc_I4_0);
                    break;

                case InstructionCode.CilLdc_I4_S:
                    SimplifyFromSByte(ins, InstructionCode.CilLdc_I4);
                    break;
                case InstructionCode.CilBr_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBr);
                    break;
                case InstructionCode.CilBrfalse_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBrfalse);
                    break;
                case InstructionCode.CilBrtrue_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBrtrue);
                    break;
                case InstructionCode.CilBeq_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBeq);
                    break;
                case InstructionCode.CilBge_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBge);
                    break;
                case InstructionCode.CilBgt_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBgt);
                    break;
                case InstructionCode.CilBle_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBle);
                    break;
                case InstructionCode.CilBlt_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBlt);
                    break;
                case InstructionCode.CilBne_Un_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBne_Un);
                    break;
                case InstructionCode.CilBge_Un_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBge_Un);
                    break;
                case InstructionCode.CilBgt_Un_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBgt_Un);
                    break;
                case InstructionCode.CilBle_Un_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBle_Un);
                    break;
                case InstructionCode.CilBlt_Un_S:
                    SimplifyFromSByte(ins, InstructionCode.CilBlt_Un);
                    break;
            }

            //remove jumping branches that go to the next instruction
            if (ins.Code == InstructionCode.CilBr && ins.Operand0 == ins.Next)
            {
                chain.Remove();
                return false;
            }

            return true;
        }

        static void Simplify(IInstruction i, InstructionCode op, object operand)
        {
            i.Code = op;
            i.Operand0 = operand;
        }

        static void SimplifyFromSByte(IInstruction i, InstructionCode op)
        {
            i.Code = op;
            if (!(i.Operand0 is IInstruction))
                i.Operand0 = (int)(sbyte)i.Operand0;
        }

        static void SimplifyFromByte(IInstruction i, InstructionCode op)
        {
            i.Code = op;
            i.Operand0 = (int)(byte)i.Operand0;
        }

        public override void Compile(IMethodContext context)
        {
            var ic = InstructionChain.FromInstructions(context.Instruction);

            while (!ic.EndOfInstructions)
            {
                if (Compile(context, ic))
                    ic.Increment();
            }

            ic.Reset();
            
            for (; !ic.EndOfInstructions; ic++)
            {
                var ins = ic.Instruction;

                if (ins.Code != InstructionCode.CilStloc)
                    continue;

                var next = ins.Next;
                if (next == null || next.Code != InstructionCode.CilLdloc || ins.Operand0 != next.Operand0)
                    continue;

                //store and immediate load can be replaced with NOOP
                ic.Remove(); // store
                ic.Remove(); // load
            }
        }
    }
}