using System.Linq;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public class CodeSimplifier : CodeCompilerStage
    {
        public CodeSimplifier(CodeCompiler compiler)
            : base(compiler)
        {
        }

        public override ICodeContext Compile(ICodeContext context, MethodBody body)
        {
            foreach (var i in body.Instructions.Where(i => i.OpCode.OpCodeType == OpCodeType.Macro))
            {
                switch (i.OpCode.Code)
                {
                    case Code.Ldarg_0:
                        Simplify(i, OpCodes.Ldarg, body.Method.Parameters[0]);
                        break;
                    case Code.Ldarg_1:
                        Simplify(i, OpCodes.Ldarg, body.Method.Parameters[1]);
                        break;
                    case Code.Ldarg_2:
                        Simplify(i, OpCodes.Ldarg, body.Method.Parameters[2]);
                        break;
                    case Code.Ldarg_3:
                        Simplify(i, OpCodes.Ldarg, body.Method.Parameters[3]);
                        break;
                    case Code.Ldloc_0:
                        Simplify(i, OpCodes.Ldloc, body.Variables[0]);
                        break;
                    case Code.Ldloc_1:
                        Simplify(i, OpCodes.Ldloc, body.Variables[1]);
                        break;
                    case Code.Ldloc_2:
                        Simplify(i, OpCodes.Ldloc, body.Variables[2]);
                        break;
                    case Code.Ldloc_3:
                        Simplify(i, OpCodes.Ldloc, body.Variables[3]);
                        break;
                    case Code.Stloc_0:
                        Simplify(i, OpCodes.Stloc, body.Variables[0]);
                        break;
                    case Code.Stloc_1:
                        Simplify(i, OpCodes.Stloc, body.Variables[1]);
                        break;
                    case Code.Stloc_2:
                        Simplify(i, OpCodes.Stloc, body.Variables[2]);
                        break;
                    case Code.Stloc_3:
                        Simplify(i, OpCodes.Stloc, body.Variables[3]);
                        break;
                    case Code.Ldarg_S:
                        i.OpCode = OpCodes.Ldarg;
                        break;
                    case Code.Ldarga_S:
                        SimplifyFromByte(i, OpCodes.Ldarga);
                        break;
                    case Code.Starg_S:
                        i.OpCode = OpCodes.Starg;
                        break;
                    case Code.Ldloc_S:
                        i.OpCode = OpCodes.Ldloc;
                        break;
                    case Code.Ldloca_S:
                        Simplify(i, OpCodes.Ldloca, body.Variables[(sbyte)i.Operand]);
                        break;
                    case Code.Stloc_S:
                        i.OpCode = OpCodes.Stloc;
                        break;
                    case Code.Ldc_I4_M1:
                        Simplify(i, OpCodes.Ldc_I4, -1);
                        break;
                    case Code.Ldc_I4_0:
                        Simplify(i, OpCodes.Ldc_I4, 0);
                        break;
                    case Code.Ldc_I4_1:
                        Simplify(i, OpCodes.Ldc_I4, 1);
                        break;
                    case Code.Ldc_I4_2:
                        Simplify(i, OpCodes.Ldc_I4, 2);
                        break;
                    case Code.Ldc_I4_3:
                        Simplify(i, OpCodes.Ldc_I4, 3);
                        break;
                    case Code.Ldc_I4_4:
                        Simplify(i, OpCodes.Ldc_I4, 4);
                        break;
                    case Code.Ldc_I4_5:
                        Simplify(i, OpCodes.Ldc_I4, 5);
                        break;
                    case Code.Ldc_I4_6:
                        Simplify(i, OpCodes.Ldc_I4, 6);
                        break;
                    case Code.Ldc_I4_7:
                        Simplify(i, OpCodes.Ldc_I4, 7);
                        break;
                    case Code.Ldc_I4_8:
                        Simplify(i, OpCodes.Ldc_I4, 8);
                        break;
                    case Code.Ldc_I4_S:
                        SimplifyFromSByte(i, OpCodes.Ldc_I4);
                        break;
                    case Code.Br_S:
                        SimplifyFromSByte(i, OpCodes.Br);
                        break;
                    case Code.Brfalse_S:
                        SimplifyFromSByte(i, OpCodes.Brfalse);
                        break;
                    case Code.Brtrue_S:
                        SimplifyFromSByte(i, OpCodes.Brtrue);
                        break;
                    case Code.Beq_S:
                        SimplifyFromSByte(i, OpCodes.Beq);
                        break;
                    case Code.Bge_S:
                        SimplifyFromSByte(i, OpCodes.Bge);
                        break;
                    case Code.Bgt_S:
                        SimplifyFromSByte(i, OpCodes.Bgt);
                        break;
                    case Code.Ble_S:
                        SimplifyFromSByte(i, OpCodes.Ble);
                        break;
                    case Code.Blt_S:
                        SimplifyFromSByte(i, OpCodes.Blt);
                        break;
                    case Code.Bne_Un_S:
                        SimplifyFromSByte(i, OpCodes.Bne_Un);
                        break;
                    case Code.Bge_Un_S:
                        SimplifyFromSByte(i, OpCodes.Bge_Un);
                        break;
                    case Code.Bgt_Un_S:
                        SimplifyFromSByte(i, OpCodes.Bgt_Un);
                        break;
                    case Code.Ble_Un_S:
                        SimplifyFromSByte(i, OpCodes.Ble_Un);
                        break;
                    case Code.Blt_Un_S:
                        SimplifyFromSByte(i, OpCodes.Blt_Un);
                        break;
                }
            }

            context = body.Instructions.Aggregate(context, this.Compile);

            return base.Compile(context, body);
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

        public override ICodeContext Compile(ICodeContext context, Instruction instruction)
        {
            //remove jumping branches that go to the next instruction
            if (instruction.OpCode.Code == Code.Br && instruction.Operand == instruction.Next)
            {
                instruction.OpCode = OpCodes.Nop;
                instruction.Operand = null;
            }

            return base.Compile(context, instruction);
        }
    }
}