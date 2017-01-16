using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public class MethodContext : IMethodContext
    {
        public string OutputFile { get; set; }

        public MethodContext(MethodDefinition method)
        {
            this.Method = method;
            this.Parameters = new List<IInstruction>(method.Parameters.Count);
            this.Variables = new List<IInstruction>();
            this.VariableDefinitions = new List<IVariableDefinition>();
            this.Code = new List<byte>();
            this.BasicBlocks = new BasicBlocks();
        }

        private int _lastLabel = 1;

        public int StackSize { get; private set; }

        public List<byte> Code { get; }

        public IList<IInstruction> Variables { get; set; }
        public IList<IVariableDefinition> VariableDefinitions { get; set; }

        public IInstruction ReturnType { get; set; }
        public IList<IInstruction> Parameters { get; set; }
        public IList<IInstruction> Locals { get; set; }

        public IInstruction Instruction { get; set; }
        public MethodDefinition Method { get; }
        public BasicBlocks BasicBlocks { get; }

        private int _registers = 0;

        public IRegister AllocateDestReg(StackType type = StackType.Imm32)
        {
            return new Register(_registers++);
        }
    }
}