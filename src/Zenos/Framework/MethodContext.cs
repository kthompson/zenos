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
            this.Parameters = new List<Instruction>(method.Parameters.Count);
            this.Variables = new List<Instruction>();
            this.VariableDefinitions = new List<IVariableDefinition>();
            this.Code = new List<byte>();
            //this.BasicBlocks = new BasicBlocks();
        }

        public int StackSize { get; private set; }

        public List<byte> Code { get; }

        public IList<Instruction> Variables { get; set; }
        public IList<IVariableDefinition> VariableDefinitions { get; set; }

        public Instruction ReturnType { get; set; }
        public IList<Instruction> Parameters { get; set; }
        public IList<Instruction> Locals { get; set; }

        public Instruction Instruction { get; set; }
        public MethodDefinition Method { get; }
        //public BasicBlocks BasicBlocks { get; }

        private int _registers = 0;

        public Register AllocateDestReg(StackType type = StackType.Imm32)
        {
            return null;
            //TODO: return RegisterModule.create(_registers++);
        }
    }
}