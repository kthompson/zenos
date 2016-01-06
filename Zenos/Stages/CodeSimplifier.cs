using System;
using System.ComponentModel;
using System.Linq;
using Mono.Cecil.Cil;
using System.Collections.Generic;

namespace Zenos.Framework
{
    public class CilToZil : CodeCompilerStage
    {
        delegate void Simplifier(IMethodContext context, IInstruction ins);

        static readonly Dictionary<InstructionCode, Simplifier> Simplifiers = new Dictionary<InstructionCode, Simplifier>();

        static void AddSimplifier(Simplifier simplifier, params InstructionCode[] codes)
        {
            foreach (var code in codes)
            {
                Simplifiers[code] = simplifier;
            }
        }

        static void AddSByteSimplifier(InstructionCode sByteIns, InstructionCode dest)
        {
            AddSimplifier((context, ins) => SimplifyFromSByte(ins, dest), sByteIns);
        }

        static CilToZil()
        {
            AddSimplifier(
                (context, ins) => Simplify(ins, InstructionCode.ZilLoad, context.Parameters[ins.Code - InstructionCode.CilLdarg_0]),
                InstructionCode.CilLdarg_0, 
                InstructionCode.CilLdarg_1, 
                InstructionCode.CilLdarg_2,
                InstructionCode.CilLdarg_3);

            AddSimplifier(
                (context, ins) => Simplify(ins, InstructionCode.ZilLoad, context.Locals[ins.Code - InstructionCode.CilLdloc_0]),
                InstructionCode.CilLdloc_0,
                InstructionCode.CilLdloc_1, 
                InstructionCode.CilLdloc_2,
                InstructionCode.CilLdloc_3);

            //loads the address of a variable onto the stack
            AddSimplifier(
               (context, ins) => Simplify(ins, InstructionCode.ZilLoad, context.Parameters[(sbyte)ins.Operand0]),
               InstructionCode.CilLdarg_S,
               InstructionCode.CilLdarga_S);


            AddSimplifier(
                (context, ins) => Simplify(ins, InstructionCode.ZilLoad, context.Locals[(sbyte)ins.Operand0]),
                InstructionCode.CilLdloc_S,
                InstructionCode.CilLdloca_S);


            AddSimplifier(
                (context, ins) => Simplify(ins, InstructionCode.ZilStore, context.Locals[ins.Code - InstructionCode.CilStloc_0]),
                InstructionCode.CilStloc_0,
                InstructionCode.CilStloc_1,
                InstructionCode.CilStloc_2,
                InstructionCode.CilStloc_3);

            AddSimplifier(
                (context, ins) => Simplify(ins, InstructionCode.ZilStore, context.Parameters[(sbyte)ins.Operand0]),
                InstructionCode.CilStarg_S);

            AddSimplifier(
                (context, ins) => Simplify(ins, InstructionCode.ZilStore, context.Locals[(sbyte)ins.Operand0]),
                InstructionCode.CilStloc_S);


            AddSimplifier(
                (context, ins) => Simplify(ins, InstructionCode.ZilLoad, ins.Code - InstructionCode.CilLdc_I4_0),
                InstructionCode.CilLdc_I4_M1,
                InstructionCode.CilLdc_I4_0,
                InstructionCode.CilLdc_I4_1,
                InstructionCode.CilLdc_I4_2,
                InstructionCode.CilLdc_I4_3,
                InstructionCode.CilLdc_I4_4,
                InstructionCode.CilLdc_I4_5,
                InstructionCode.CilLdc_I4_6,
                InstructionCode.CilLdc_I4_7,
                InstructionCode.CilLdc_I4_8);

            AddSimplifier(
               (context, ins) => Simplify(ins, InstructionCode.ZilLoad, (int)(sbyte)ins.Operand0),
               InstructionCode.CilLdc_I4_S);

            AddSByteSimplifier(InstructionCode.CilBrtrue_S, InstructionCode.CilBrtrue);

            AddSByteSimplifier(InstructionCode.CilBr_S, InstructionCode.CilBr);
            AddSByteSimplifier(InstructionCode.CilBrfalse_S, InstructionCode.CilBrfalse);
            AddSByteSimplifier(InstructionCode.CilBrtrue_S, InstructionCode.CilBrtrue);
            AddSByteSimplifier(InstructionCode.CilBeq_S, InstructionCode.CilBeq);
            AddSByteSimplifier(InstructionCode.CilBge_S, InstructionCode.CilBge);
            AddSByteSimplifier(InstructionCode.CilBgt_S, InstructionCode.CilBgt);
            AddSByteSimplifier(InstructionCode.CilBle_S, InstructionCode.CilBle);
            AddSByteSimplifier(InstructionCode.CilBlt_S, InstructionCode.CilBlt);
            AddSByteSimplifier(InstructionCode.CilBne_Un_S, InstructionCode.CilBne_Un);
            AddSByteSimplifier(InstructionCode.CilBge_Un_S, InstructionCode.CilBgt);
            AddSByteSimplifier(InstructionCode.CilBgt_Un_S, InstructionCode.CilBgt_Un);
            AddSByteSimplifier(InstructionCode.CilBle_Un_S, InstructionCode.CilBle_Un);
            AddSByteSimplifier(InstructionCode.CilBlt_Un_S, InstructionCode.CilBlt_Un);

            AddSimplifier(
               (context, ins) => Simplify(ins, InstructionCode.ZilLoad, (int)(sbyte)ins.Operand0),
               InstructionCode.CilLdc_I4_S);

            AddSimplifier(
               (context, ins) => Simplify(ins, InstructionCode.ZilLoad, (int)ins.Operand0),
               InstructionCode.CilLdc_I4);

            AddSimplifier(
               (context, ins) => Simplify(ins, InstructionCode.ZilLoad, (long)ins.Operand0),
               InstructionCode.CilLdc_I8);
        }

        public virtual bool Compile(IMethodContext context, InstructionChain chain)
        {
            var ins = chain.Instruction;

            Simplifier simplifier;
            if (Simplifiers.TryGetValue(ins.Code, out simplifier))
                simplifier(context, ins);
            else
            {
                // default operation
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
                i.Operand0 = ((sbyte)i.Operand0);
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

                if (ins.Code != InstructionCode.ZilStore)
                    continue;

                var next = ins.Next;
                if (next == null || next.Code != InstructionCode.ZilLoad || ins.Operand0 != next.Operand0)
                    continue;

                //store and immediate load can be replaced with NOOP
                ic.Remove(); // store
                ic.Remove(); // load
            }
        }
    }
}