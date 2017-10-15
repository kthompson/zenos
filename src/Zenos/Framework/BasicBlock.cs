using System;
using System.Collections;
using System.Collections.Generic;

namespace Zenos.Framework
{
    //public class BasicBlocks : IEnumerable<BasicBlock>
    //{
    //    public int Count => _blocks.Count;

    //    private readonly Dictionary<int, BasicBlock> _blockOffsetMap;
    //    private readonly SortedSet<BasicBlock> _blocks;

    //    public BasicBlocks()
    //    {
    //        _blockOffsetMap = new Dictionary<int, BasicBlock>();
    //        _blocks = new SortedSet<BasicBlock>();
    //    }

    //    public BasicBlock Add(BasicBlock block)
    //    {
    //        _blockOffsetMap[block.BlockId] = block;
    //        _blocks.Add(block);
    //        return block;
    //    }



    //    public BasicBlock GetOrCreateByTarget(Instruction instruction) =>
    //        GetByOffset(instruction.Offset) ?? Add(new BasicBlock(instruction));

    //    public BasicBlock GetByOffset(int offset)
    //    {
    //        BasicBlock block;
    //        if (_blockOffsetMap.TryGetValue(offset, out block))
    //            return block;

    //        return null;
    //    }

    //    public IEnumerator<BasicBlock> GetEnumerator()
    //    {
    //        return _blocks.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}

    //public class BasicBlockComparer : IComparer<BasicBlock>
    //{
    //    public int Compare(BasicBlock x, BasicBlock y) => 
    //        x.BlockId.CompareTo(y.BlockId);
    //}

    //public class BasicBlock : IEnumerable<Instruction>, IComparable<BasicBlock>
    //{
    //    public int BlockId { get; }
    //    public string Label => $"BB_{BlockId:X4}";
    //    public Instruction Instruction { get; }
    //    public HashSet<BasicBlock> OutBasicBlocks { get; }
    //    public HashSet<BasicBlock> InBasicBlocks { get; }

    //    public BasicBlock(Instruction instruction)
    //    {
    //        this.BlockId = instruction.Offset;
    //        this.Instruction = instruction;
    //        this.OutBasicBlocks = new HashSet<BasicBlock>();
    //        this.InBasicBlocks = new HashSet<BasicBlock>();
    //    }

    //    public IEnumerator<Instruction> GetEnumerator() => 
    //        this.Instruction.GetEnumerator();

    //    public int CompareTo(BasicBlock other) => 
    //        this.BlockId.CompareTo(other.BlockId);

    //    public override string ToString()
    //    {
    //        return $"BasicBlock[{Label}, I{InBasicBlocks.Count}, O{OutBasicBlocks.Count}]";
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}
}