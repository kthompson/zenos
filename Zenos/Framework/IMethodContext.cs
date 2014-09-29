using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{

    //MonoCompile
    public interface IMethodContext : IDisposable
    {
        ITypeContext Context { get; }
        bool IsDisposed { get; }

        Sections Sections { get; }
        string OutputFile { get; set; }
        Section Text { get; }
        Section Data { get; }
        int StackSize { get; }

        IList<IInstruction> Variables { get; set; }
        IList<IVariableDefinition> VariableDefinitions { get; set; }

        IInstruction ReturnType { get; set; }

        IList<IInstruction> Parameters { get; set; }
        IList<IInstruction> Locals { get; set; }


        string CreateLabel(string prefix = null);
        string CreateTemporary();

        IInstruction Instruction { get; set; }
        MethodDefinition Method { get; }
        BasicBlock CurrentBasicBlock { get; set; }
        int next_vreg { get; set; }

        IDictionary<IRegister, IInstruction> VRegisterToInstruction { get; }

        /* 
         * This variable represents the hidden argument holding the vtype
         * return address. If the method returns something other than a vtype, or
         * the vtype is returned in registers this is NULL.
         */
        IInstruction vret_addr { get; set; }
        bool ret_var_is_local { get; set; }
        int arch_eh_jit_info { get; set; }

        BasicBlock start_bblock { get; set; }
        BasicBlock end_bblock { get; set; }
        int num_bblocks { get; set; }
        BasicBlock bblock { get; set; }
        int cil_start { get; set; }
        int real_offset { get; set; }
        Dictionary<int, BasicBlock> cil_offset_to_bb { get; set; }
        BasicBlock bb_init { get; set; }
    }

    public interface IVariableDefinition
    {
        TypeReference VariableType { get; set;  }
        int Index { get; set; }
        IRegister Register { get; set; }
        IRegister VirtualRegister { get; set; }
    }

    class VariableDefinition : IVariableDefinition
    {
        public TypeReference VariableType { get; set; }
        public int Index { get; set; }
        public IRegister Register { get; set; }
        public IRegister VirtualRegister { get; set; }

        public override string ToString()
        {
            return string.Format("var{0}", this.Index);
        }
    }

    public class BasicBlock
    {
        public IInstruction last_ins;

        /* the next basic block in the order it appears in IL */
        public BasicBlock next_bb;
        public IInstruction code;
        public IInstruction cil_code;
        public int block_num;
        public List<BasicBlock> out_bb { get; set; }
        public List<BasicBlock> in_bb { get; set; }
        public int real_offset;
        public long cil_length;
        public List<IInstruction> in_stack { get; set; }
        public List<IInstruction> out_stack { get; set; }

        public BasicBlock()
        {
            this.out_bb = new List<BasicBlock>();
            this.out_stack = new List<IInstruction>();
            this.in_stack = new List<IInstruction>();
            this.in_bb = new List<BasicBlock>();
        }

        public override string ToString()
        {
            return string.Format("BasicBlock[N{0}, I{1}, O{2}]", block_num, in_bb.Count, out_bb.Count);
        }
    }

    [Flags]
    internal enum BasicBlockFlags
    {
        BB_VISITED = 1 << 0,
        BB_REACHABLE = 1 << 1,
        BB_EXCEPTION_DEAD_OBJ = 1 << 2,
        BB_EXCEPTION_UNSAFE = 1 << 3,
        BB_EXCEPTION_HANDLER = 1 << 4,

        /* for Native Client, mark the blocks that can be jumped to indirectly */
        BB_INDIRECT_JUMP_TARGET = 1 << 5
    }
}
