using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Zenos.Framework
{
    public interface IMethodContext : IDisposable
    {
        ITypeContext Context { get; }
        bool IsDisposed { get; }

        Sections Sections { get; }
        string OutputFile { get; set; }
        Section Text { get; }
        Section Data { get; }
        int StackSize { get; }

        IInstruction[] Variables { get; set; }
        VariableDefinition[] VariableDefinitions { get; set; }

        IInstruction ReturnType { get; set; }

        IInstruction[] Parameters { get; set; }
        ParameterDefinition[] ParameterDefinitions { get; set; }

        string CreateLabel(string prefix = null);
        string CreateTemporary();

        IInstruction Instruction { get; set; }
        MethodDefinition Method { get; }
        BasicBlock CurrentBasicBlock { get; set; }
        int next_vreg { get; set; }
    }

    public class BasicBlock
    {
        public IInstruction last_ins;

        /* the next basic block in the order it appears in IL */
        BasicBlock next_bb;
        public IInstruction code;
    }
}