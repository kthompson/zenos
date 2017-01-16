using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    //MonoCompile
    public interface IMethodContext
    {
        List<byte> Code { get; }

        IList<IInstruction> Variables { get; set; }

        IInstruction ReturnType { get; set; }

        IList<IInstruction> Parameters { get; set; }
        IList<IInstruction> Locals { get; set; }

        IInstruction Instruction { get; set; }
        MethodDefinition Method { get; }
        
        BasicBlocks BasicBlocks { get; }

        IRegister AllocateDestReg(StackType type = StackType.Imm32);
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
