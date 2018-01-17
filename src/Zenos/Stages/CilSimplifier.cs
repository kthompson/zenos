using System;
using System.ComponentModel;
using System.Linq;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using Microsoft.FSharp.Core;
using Mono.Cecil;

namespace Zenos.Framework
{
    //public class CilSimplifier : Compiler<IMethodContext>
    //{

    //    static void AddSByteSimplifier(InstructionCode sByteIns, InstructionCode dest)
    //    {
    //        AddSimplifier((context, ins) => SimplifyFromSByte(ins, dest), sByteIns);
    //    }

    //    static CilSimplifier()
    //    {
    //        //AddSimplifier(
    //        //    (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Load), context.Parameters[ins.Code - InstructionCode.CilLdarg_0]),
    //        //    InstructionCode.NewCil(Code.Ldarg_0),
    //        //    InstructionCode.NewCil(Code.Ldarg_1),
    //        //    InstructionCode.NewCil(Code.Ldarg_2),
    //        //    InstructionCode.NewCil(Code.Ldarg_3));

    //        //AddSimplifier(
    //        //    (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Load), context.Locals[ins.OpCode - InstructionCode.CilLdloc_0]),
    //        //    InstructionCode.NewCil(Code.Ldloc_0),
    //        //    InstructionCode.NewCil(Code.Ldloc_1),
    //        //    InstructionCode.NewCil(Code.Ldloc_2),
    //        //    InstructionCode.NewCil(Code.Ldloc_3));

    //        //loads the address of a variable onto the stack
    //        //AddSimplifier(
    //        //   (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Load), context.Parameters[(sbyte)ins.Operand0]),
    //        //   InstructionCode.NewCil(Code.Ldarg_S),
    //        //   InstructionCode.NewCil(Code.Ldarga_S));


    //        //AddSimplifier(
    //        //    (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Load), context.Locals[(sbyte)ins.Operand0]),
    //        //    InstructionCode.NewCil(Code.Ldloc_S),
    //        //    InstructionCode.NewCil(Code.Ldloca_S));


    //        //AddSimplifier(
    //        //    (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Store), context.Locals[ins.OpCode - InstructionCode.CilStloc_0]),
    //        //    InstructionCode.NewCil(Code.Stloc_0),
    //        //    InstructionCode.NewCil(Code.Stloc_1),
    //        //    InstructionCode.NewCil(Code.Stloc_2),
    //        //    InstructionCode.NewCil(Code.Stloc_3));

    //        //AddSimplifier(
    //        //    (context, ins) =>
    //        //    {
    //        //        if (FSharpOption<Operand>.get_IsNone(ins.Operand0))
    //        //            return ins;

    //        //        var operand = ins.Operand0.Value;
    //        //        if (operand.IsSByte)
    //        //        {
    //        //            var sb = ((Operand.SByte) operand).Item;
    //        //            return Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Store), Operand.NewInstruction(context.Parameters[sb]));
    //        //        }

    //        //        if (operand.IsParameter)
    //        //        {
    //        //            var paramDef = ((Operand.Parameter) operand).Item;
    //        //            return Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Store), Operand.NewInstruction(context.Parameters[paramDef.Index]));
    //        //        }

    //        //        return ins;
    //        //    },
    //        //    InstructionCode.NewCil(Code.Starg_S));

    //        //AddSimplifier(
    //        //    (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Store), context.Locals[(sbyte)ins.Operand0]),
    //        //    InstructionCode.NewCil(Code.Stloc_S));


    //        //AddSimplifier(
    //        //    (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Load), ins.Code - InstructionCode.NewCil(Code.Ldc_I4_0)),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_M1),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_0),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_1),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_2),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_3),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_4),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_5),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_6),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_7),
    //        //    InstructionCode.NewCil(Code.Ldc_I4_8));

    //        //AddSimplifier(
    //        //   (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Load), (int)(sbyte)ins.Operand0),
    //        //   InstructionCode.NewCil(Code.Ldc_I4_S));

    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Brtrue_S), InstructionCode.NewCil(Code.Brtrue));

    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Br_S), InstructionCode.NewCil(Code.Br));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Brfalse_S), InstructionCode.NewCil(Code.Brfalse));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Brtrue_S), InstructionCode.NewCil(Code.Brtrue));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Beq_S), InstructionCode.NewCil(Code.Beq));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Bge_S), InstructionCode.NewCil(Code.Bge));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Bgt_S), InstructionCode.NewCil(Code.Bgt));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Ble_S), InstructionCode.NewCil(Code.Ble));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Blt_S), InstructionCode.NewCil(Code.Blt));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Bne_Un_S), InstructionCode.NewCil(Code.Bne_Un));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Bge_Un_S), InstructionCode.NewCil(Code.Bgt));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Bgt_Un_S), InstructionCode.NewCil(Code.Bgt_Un));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Ble_Un_S), InstructionCode.NewCil(Code.Ble_Un));
    //        AddSByteSimplifier(InstructionCode.NewCil(Code.Blt_Un_S), InstructionCode.NewCil(Code.Blt_Un));

    //        //AddSimplifier(
    //        //   (context, ins) => Simplify(ins, InstructionCode.NewZenos(ZenosOpCode.Load), (int)(sbyte)ins.Operand0),
    //        //   InstructionCode.NewCil(Code.Ldc_I4_S));

    //        //AddSimplifier(
    //        //   (context, ins) => ins.Code = InstructionCode.NewZenos(ZenosOpCode.Load),
    //        //   InstructionCode.NewCil(Code.Ldc_I4));

    //        //AddSimplifier(
    //        //   (context, ins) => ins.Code = InstructionCode.NewZenos(ZenosOpCode.Load),
    //        //   InstructionCode.NewCil(Code.Ldc_I8));

    //        //AddSimplifier(
    //        //    (context, ins) => ins.Code = InstructionCode.NewZenos(ZenosOpCode.PushConstant),
    //        //    InstructionCode.NewCil(Code.Ldc_R4),
    //        //    InstructionCode.NewCil(Code.Ldc_R8));
    //    }

    //    public virtual bool Compile(IMethodContext context, Instruction ins)
    //    {

    //        Simplifier simplifier;
    //        if (Simplifiers.TryGetValue(ins.OpCode, out simplifier))
    //            simplifier(context, ins);
    //        else
    //        {
    //            // default operation
    //        }

    //        //TODO: we cant do this unless we fix up jumps
    //        //remove jumping branches that go to the next instruction
    //        //if (ins.Code == InstructionCode.CilBr && ins.Operand0 == ins.Next)
    //        //{
    //        //    chain.Remove();
    //        //    return false;
    //        //}

    //        return true;
    //    }

    //    static Instruction Simplify(Instruction i, InstructionCode op, Operand operand)
    //    {
    //        //TODO: make this work with F#
    //        //i.Code = op;
    //        //i.Operand0 = operand;
    //        return i;
    //    }

    //    static Instruction SimplifyFromSByte(Instruction i, InstructionCode op)
    //    {
    //        //TODO: make this work with F#
    //        //i.Code = op;
    //        //if (!(i.Operand0 is Instruction))
    //        //    i.Operand0 = ((sbyte)i.Operand0);

    //        return i;
    //    }

    //    public override IMethodContext Compile(IMethodContext context)
    //    {
    //        //foreach (var inst in context.Instruction)
    //        //{
    //        //    Compile(context, inst);
    //        //}
            
    //        //TODO: we cant do this unless we fix up jumps
    //        //for (; !ic.EndOfInstructions; ic++)
    //        //{
    //        //    var ins = ic.Instruction;

    //        //    if (ins.Code != InstructionCode.ZilStore)
    //        //        continue;

    //        //    var next = ins.Next;
    //        //    if (next == null || next.Code != InstructionCode.ZilLoad || ins.Operand0 != next.Operand0)
    //        //        continue;

    //        //    //store and immediate load can be replaced with NOOP
    //        //    ic.Remove(); // store
    //        //    ic.Remove(); // load
    //        //}

    //        return context;
    //    }
    //}
}