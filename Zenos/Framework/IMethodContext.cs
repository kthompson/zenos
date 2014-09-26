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
        BasicBlock next_bb;
        public IInstruction code;
    }
}