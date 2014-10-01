using System;
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

    [Flags]
    public enum InstructionFlags
    {
        HasMethod = 1,
        Init = 1, /* in localloc */
        SingleStepLoc = 1, /* in SEQ_POINT */
        IsDead = 2,
        Tailcall = 4,
        Volatile = 4,
        Notypecheck = 4,
        NonemptyStack = 4, /* in SEQ_POINT */
        Unaligned = 8,
        CfoldTaken = 8, /* On branches */
        CfoldNotTaken = 16, /* On branches */
        DefinitionHasSideEffects = 8,
        /* the address of the variable has been taken */
        Indirect = 16,
        Norangecheck = 16,
        /* On loads, the source address can be null */
        Fault = 32,
        /* 
         * On variables, identifies LMF variables. These variables have a dummy type (int), but
         * require stack space for a MonoLMF struct.
         */
        Lmf = 32,
        /* On loads, the source address points to a constant value */
        InvariantLoad = 64,
        /* On variables, the variable needs GC tracking */
        GcTrack = 128,
        /*
         * Set on instructions during code emission which make calls, i.e. OP_CALL, OP_THROW.
         * backend.pc_offset will be set to the pc offset at the end of the native call instructions.
         */
        GcCallsite = 128
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