using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public class MethodContext : IMethodContext
    {
        public ITypeContext Context { get; private set; }
        public Sections Sections { get; private set; }
        public string OutputFile { get; set; }
        public bool IsDisposed { get; private set; }

        public MethodContext(ITypeContext context, MethodDefinition method)
        {
            this.Context = context;
            this.Method = method;
            this.Sections = new Sections();
            this.Variables = new List<IInstruction>();
            this.VariableDefinitions = new List<IVariableDefinition>();
            this.VRegisterToInstruction = new Dictionary<IRegister, IInstruction>();
        }

        private int _lastLabel = 1;

        public Section Text
        {
            get { return this.Sections["text"]; }
        }

        public Section Data
        {
            get { return this.Sections["rdata"]; }
        }

        public int StackSize { get; private set; }

        public IList<IInstruction> Variables { get; set; }
        public IList<IVariableDefinition> VariableDefinitions { get; set; }

        public IInstruction ReturnType { get; set; }
        public IList<IInstruction> Parameters { get; set; }
        public IInstruction[] Arguments { get; set; }
        public IList<IInstruction> Locals { get; set; }

        public string CreateLabel(string prefix = null)
        {
            return (prefix ?? "L") + (_lastLabel++).ToString("D4");
        }

        public string CreateTemporary()
        {
            this.StackSize += 4;
            return string.Format("{0}(%ebp)", this.StackSize);
        }

        public IInstruction Instruction { get; set; }
        public MethodDefinition Method { get; private set; }
        public BasicBlock CurrentBasicBlock { get; set; }
        public int next_vreg { get; set; }
        public IDictionary<IRegister, IInstruction> VRegisterToInstruction { get; private set; }
        public IInstruction vret_addr { get; set; }
        public bool ret_var_is_local { get; set; }
        public int arch_eh_jit_info { get; set; }
        public BasicBlock start_bblock { get; set; }
        public BasicBlock end_bblock { get; set; }
        public int num_bblocks { get; set; }
        public BasicBlock bblock {
            get { return this.CurrentBasicBlock; }
            set { this.CurrentBasicBlock = value; }
        }

        public int cil_start { get; set; }
        public int real_offset { get; set; }
        public Dictionary<int, BasicBlock> cil_offset_to_bb { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (!disposing)
                return;

            foreach (var section in this.Sections)
            {
                section.Dispose();
            }

            this.Context = null;
            this.IsDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}