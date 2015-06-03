using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public interface IInstruction
    {
        InstructionCode Code { get; set; }
        StackType StackType { get; set; }
        InstructionFlags Flags { get; set; }

        int Offset { get; set; }

        IRegister Destination { get; set; }
        IRegister Source1 { get; set; }
        IRegister Source2 { get; set; }
        IRegister Source3 { get; set; }
    
        IInstruction Previous { get; set; }
        IInstruction Next { get; set; }

        object Operand0 { get; set; }
        object Operand1 { get; set; }
        object Operand2 { get; set; }

        Instruction SourceInstruction { get; set; }
        TypeDefinition klass { get; set; }
    }


    public static class DictionaryMixins
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
        {
            if (@this.ContainsKey(key))
                return @this[key];

            return default(TValue);
        }
    }

    public static class InstructionMixins
    {
        public static TypeReference inst_vtype(this IInstruction instruction)
        {
            return (TypeReference)instruction.Operand1;
        }

        public static void set_inst_vtype(this IInstruction instruction, TypeReference value)
        {
            instruction.Operand1 = value;
        }

        public static int inst_c0(this IInstruction instruction)
        {
            return (int)instruction.Operand0;
        }

        public static void set_inst_c0(this IInstruction instruction, int value)
        {
            instruction.Operand0 = value;
        }

        public static int inst_c1(this IInstruction instruction)
        {
            return (int)instruction.Operand1;
        }


        public static IInstruction inst_i0(this IInstruction instruction)
        {
            return (IInstruction)instruction.Operand0;
        }

        public static IInstruction inst_i1(this IInstruction instruction)
        {
            return (IInstruction)instruction.Operand1;
        }

        public static BasicBlock[] inst_many_bb(this IInstruction instruction)
        {
            return (BasicBlock[])instruction.Operand1;
        }

        public static BasicBlock inst_target_bb(this IInstruction instruction)
        {
            return (BasicBlock)instruction.Operand0;
        }

        public static void set_inst_target_bb(this IInstruction instruction, BasicBlock value)
        {
            instruction.Operand0 = value;
        }

        public static BasicBlock inst_true_bb(this IInstruction instruction)
        {
            return instruction.inst_many_bb()[0];
        }

        public static BasicBlock inst_false_bb(this IInstruction instruction)
        {
            return instruction.inst_many_bb()[1];
        }


        public static void set_inst_p0(this IInstruction instruction, IInstruction value)
        {
            instruction.Operand0 = value;
        }

        public static IInstruction inst_p0(this IInstruction instruction)
        {
            return (IInstruction)instruction.Operand0;
        }

        public static void set_inst_p1(this IInstruction instruction, IInstruction value)
        {
            instruction.Operand1 = value;
        }

        public static IInstruction inst_p1(this IInstruction instruction)
        {
            return (IInstruction)instruction.Operand1;
        }
    }
}