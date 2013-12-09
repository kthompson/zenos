using System.Linq;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public class CodeSimplifier : InstructionCompilerStage
    {

        public override IInstruction Compile(IMethodContext context, IInstruction instruction)
        {
            if (instruction.SourceInstruction.OpCode.OpCodeType != OpCodeType.Macro)
                return instruction;

            switch (instruction.Code)
            {
                case InstructionCode.CilLdarg_0:
                    Simplify(i, OpCodes.Ldarg, body.Method.Parameters[0]);
                    break;
                case InstructionCode.CilLdarg_1:
                    Simplify(i, OpCodes.Ldarg, body.Method.Parameters[1]);
                    break;
                case InstructionCode.CilLdarg_2:
                    Simplify(i, OpCodes.Ldarg, body.Method.Parameters[2]);
                    break;
                case InstructionCode.CilLdarg_3:
                    Simplify(i, OpCodes.Ldarg, body.Method.Parameters[3]);
                    break;
                case InstructionCode.CilLdloc_0:
                    Simplify(i, OpCodes.Ldloc, body.Variables[0]);
                    break;
                case InstructionCode.CilLdloc_1:
                    Simplify(i, OpCodes.Ldloc, body.Variables[1]);
                    break;
                case InstructionCode.CilLdloc_2:
                    Simplify(i, OpCodes.Ldloc, body.Variables[2]);
                    break;
                case InstructionCode.CilLdloc_3:
                    Simplify(i, OpCodes.Ldloc, body.Variables[3]);
                    break;
                case InstructionCode.CilStloc_0:
                    Simplify(i, OpCodes.Stloc, body.Variables[0]);
                    break;
                case InstructionCode.CilStloc_1:
                    Simplify(i, OpCodes.Stloc, body.Variables[1]);
                    break;
                case InstructionCode.CilStloc_2:
                    Simplify(i, OpCodes.Stloc, body.Variables[2]);
                    break;
                case InstructionCode.CilStloc_3:
                    Simplify(i, OpCodes.Stloc, body.Variables[3]);
                    break;
                case InstructionCode.CilLdarg_S:
                    i.OpCode = OpCodes.Ldarg;
                    break;
                case InstructionCode.CilLdarga_S:
                    SimplifyFromByte(i, OpCodes.Ldarga);
                    break;
                case InstructionCode.CilStarg_S:
                    i.OpCode = OpCodes.Starg;
                    break;
                case InstructionCode.CilLdloc_S:
                    i.OpCode = OpCodes.Ldloc;
                    break;
                case InstructionCode.CilLdloca_S:
                    Simplify(i, OpCodes.Ldloca, body.Variables[(sbyte)i.Operand]);
                    break;
                case InstructionCode.CilStloc_S:
                    i.OpCode = OpCodes.Stloc;
                    break;
                case InstructionCode.CilLdc_I4_M1:
                    Simplify(i, OpCodes.Ldc_I4, -1);
                    break;
                case InstructionCode.CilLdc_I4_0:
                    Simplify(i, OpCodes.Ldc_I4, 0);
                    break;
                case InstructionCode.CilLdc_I4_1:
                    Simplify(i, OpCodes.Ldc_I4, 1);
                    break;
                case InstructionCode.CilLdc_I4_2:
                    Simplify(i, OpCodes.Ldc_I4, 2);
                    break;
                case InstructionCode.CilLdc_I4_3:
                    Simplify(i, OpCodes.Ldc_I4, 3);
                    break;
                case InstructionCode.CilLdc_I4_4:
                    Simplify(i, OpCodes.Ldc_I4, 4);
                    break;
                case InstructionCode.CilLdc_I4_5:
                    Simplify(i, OpCodes.Ldc_I4, 5);
                    break;
                case InstructionCode.CilLdc_I4_6:
                    Simplify(i, OpCodes.Ldc_I4, 6);
                    break;
                case InstructionCode.CilLdc_I4_7:
                    Simplify(i, OpCodes.Ldc_I4, 7);
                    break;
                case InstructionCode.CilLdc_I4_8:
                    Simplify(i, OpCodes.Ldc_I4, 8);
                    break;
                case InstructionCode.CilLdc_I4_S:
                    SimplifyFromSByte(i, OpCodes.Ldc_I4);
                    break;
                case InstructionCode.CilBr_S:
                    SimplifyFromSByte(i, OpCodes.Br);
                    break;
                case InstructionCode.CilBrfalse_S:
                    SimplifyFromSByte(i, OpCodes.Brfalse);
                    break;
                case InstructionCode.CilBrtrue_S:
                    SimplifyFromSByte(i, OpCodes.Brtrue);
                    break;
                case InstructionCode.CilBeq_S:
                    SimplifyFromSByte(i, OpCodes.Beq);
                    break;
                case InstructionCode.CilBge_S:
                    SimplifyFromSByte(i, OpCodes.Bge);
                    break;
                case InstructionCode.CilBgt_S:
                    SimplifyFromSByte(i, OpCodes.Bgt);
                    break;
                case InstructionCode.CilBle_S:
                    SimplifyFromSByte(i, OpCodes.Ble);
                    break;
                case InstructionCode.CilBlt_S:
                    SimplifyFromSByte(i, OpCodes.Blt);
                    break;
                case InstructionCode.CilBne_Un_S:
                    SimplifyFromSByte(i, OpCodes.Bne_Un);
                    break;
                case InstructionCode.CilBge_Un_S:
                    SimplifyFromSByte(i, OpCodes.Bge_Un);
                    break;
                case InstructionCode.CilBgt_Un_S:
                    SimplifyFromSByte(i, OpCodes.Bgt_Un);
                    break;
                case InstructionCode.CilBle_Un_S:
                    SimplifyFromSByte(i, OpCodes.Ble_Un);
                    break;
                case InstructionCode.CilBlt_Un_S:
                    SimplifyFromSByte(i, OpCodes.Blt_Un);
                    break;
            }
        }

        public override void Compile(ICompilationContext context, MethodBody body)
        {
            foreach (var i in body.Instructions.Where(i => i.OpCode.OpCodeType == OpCodeType.Macro))
            {
                
            }


            this.Compile(context, body.Instructions);
        }

        static void Simplify(Instruction i, OpCode op, object operand)
        {
            i.OpCode = op;
            i.Operand = operand;
        }

        static void SimplifyFromSByte(Instruction i, OpCode op)
        {
            i.OpCode = op;
            if (!(i.Operand is Instruction))
                i.Operand = (int)(sbyte)i.Operand;
        }

        static void SimplifyFromByte(Instruction i, OpCode op)
        {
            i.OpCode = op;
            i.Operand = (int)(byte)i.Operand;
        }

        public override void Compile(ICompilationContext context, Instruction instruction)
        {
            //remove jumping branches that go to the next instruction
            if (instruction.OpCode.Code == Code.Br && instruction.Operand == instruction.Next)
            {
                instruction.OpCode = OpCodes.Nop;
                instruction.Operand = null;
            }
        }

    }
}