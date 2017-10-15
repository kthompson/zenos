using System.Collections.Generic;
using System.Linq;
using Zenos.Framework;

namespace Zenos.Stages
{
    //public class BasicBlockAssignment : Compiler<IMethodContext>
    //{
    //    public override IMethodContext Compile(IMethodContext context)
    //    {
    //        var blocks = context.BasicBlocks;
    //        blocks.GetOrCreateByTarget(context.Instruction);

    //        var leaders = CreateBlocksForBranchTargets(context, blocks);

    //        AssignInstructionsToBlocks(context, blocks, leaders);

    //        // TODO handle code that throws exceptions

    //        return base.Compile(context);
    //    }

    //    private void AssignInstructionsToBlocks(IMethodContext context, BasicBlocks blocks, HashSet<Instruction> leaders)
    //    {
    //        // TODO make work with F#
    //        //var block = blocks.First();
    //        //var inst = context.Instruction;
    //        //while (inst != null)
    //        //{
    //        //    block = blocks.GetByOffset(inst.Offset) ?? block;

    //        //    if (inst.FlowControl == FlowControl.Branch || inst.FlowControl == FlowControl.Cond_Branch)
    //        //    {
    //        //        LinkBasicBlock(block, inst.Operand0 as BasicBlock);
    //        //        LinkBasicBlock(block, inst.Operand1 as BasicBlock);
    //        //        LinkBasicBlock(block, inst.Operand2 as BasicBlock);

    //        //        // we are on the last instruction for this block so lets disconnect this instruction
    //        //        inst = inst.DetachNext();
    //        //    }
    //        //    else if (inst.Next != null && leaders.Contains(inst.Next))
    //        //    {
    //        //        var nextBlock = blocks.GetByOffset(inst.Next.Offset);

    //        //        LinkBasicBlock(block, nextBlock);

    //        //        inst = inst.InsertAfter(new Instruction
    //        //        {
    //        //            Code = InstructionCode.NewCil(CilOpCode.Br),
    //        //            Operand0 = nextBlock
    //        //        }).DetachNext();
    //        //    }
    //        //    else
    //        //    {
    //        //        inst = inst.Next;
    //        //    }
    //        //}
    //    }

    //    private static void LinkBasicBlock(BasicBlock @from, BasicBlock to)
    //    {
    //        if (to == null) return;

    //        @from.OutBasicBlocks.Add(to);
    //        to.InBasicBlocks.Add(@from);
    //    }

    //    private static HashSet<Instruction> CreateBlocksForBranchTargets(IMethodContext context, BasicBlocks blocks)
    //    {
    //        var leaders = new HashSet<Instruction>();
    //        foreach (var inst in context.Instruction)
    //        {
    //            if (inst.FlowControl != FlowControl.Branch && inst.FlowControl != FlowControl.Cond_Branch)
    //                continue;

    //            var op0 = inst.Operand0 as Instruction;
    //            if (op0 != null)
    //            {
    //                leaders.Add(op0);
    //                inst.Operand0 = blocks.GetOrCreateByTarget(op0);
    //            }

    //            var op1 = inst.Operand1 as Instruction;
    //            if (op1 != null)
    //            {
    //                leaders.Add(op1);
    //                inst.Operand1 = blocks.GetOrCreateByTarget(op1);
    //            }

    //            var op2 = inst.Operand2 as Instruction;
    //            if (op2 != null)
    //            {
    //                leaders.Add(op2);
    //                inst.Operand2 = blocks.GetOrCreateByTarget(op2);
    //            }

    //            if (inst.Next == null)
    //                continue;

    //                // next op will also be a branch target
    //            switch (inst.Code)
    //            {
    //                case InstructionCode.CilBrtrue:
    //                case InstructionCode.CilBrfalse:
    //                    leaders.Add(inst.Next);
    //                    inst.Operand1 = blocks.GetOrCreateByTarget(inst.Next);
    //                    break;

    //                default:
    //                    leaders.Add(inst.Next);
    //                    blocks.GetOrCreateByTarget(inst.Next);
    //                    break;
    //            }
    //        }

    //        return leaders;
    //    }
    //}
}