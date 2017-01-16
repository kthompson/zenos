using System;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class PopulateStackType : Compiler<IMethodContext>
    {
        public override IMethodContext Compile(IMethodContext context)
        {
            foreach (var inst in context.Instruction)
            {
                if (inst.StackType != StackType.Unknown)
                    continue;

                switch (inst.Code)
                {
                    case InstructionCode.Load:
                    {
                        inst.StackType = GetStackType(inst.Operand0);
                        break;
                    }
                }
            }
            return base.Compile(context);
        }

        private static StackType GetStackType(object obj)
        {
            var op = obj as IInstruction;
            if (op != null)
            {
                return op.StackType;
            }

            if (obj is bool || obj is short || obj is ushort || obj is int || obj is uint)
            {
                return StackType.Imm32;
            }
            else if (obj is long || obj is ulong)
            {
                return StackType.Imm64;
            }

            return StackType.Unknown;
        }
    }
}