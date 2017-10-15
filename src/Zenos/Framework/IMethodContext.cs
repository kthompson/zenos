using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;

namespace Zenos.Framework
{
    //MonoCompile
    public interface IMethodContext
    {
        List<byte> Code { get; }

        IList<Instruction> Variables { get; set; }

        Instruction ReturnType { get; set; }

        IList<Instruction> Parameters { get; set; }
        IList<Instruction> Locals { get; set; }

        Instruction Instruction { get; set; }
        MethodDefinition Method { get; }

        //BasicBlocks BasicBlocks { get; }

        Register AllocateDestReg(StackType type = StackType.Imm32);
    }

    public interface IVariableDefinition
    {
        TypeReference VariableType { get; set;  }
        int Index { get; set; }
        Register Register { get; set; }
        Register VirtualRegister { get; set; }
    }

    class VariableDefinition : IVariableDefinition
    {
        public TypeReference VariableType { get; set; }
        public int Index { get; set; }
        public Register Register { get; set; }
        public Register VirtualRegister { get; set; }

        public override string ToString() => $"var{this.Index}";
    }

    [Flags]
    public enum BasicBlockFlags
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
