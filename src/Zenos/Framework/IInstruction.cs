using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Mono.Cecil;
using CilInstruction = Mono.Cecil.Cil.Instruction;
using CilFlowControl = Mono.Cecil.Cil.FlowControl;

namespace Zenos.Framework
{
    //public interface IInstruction : IEnumerable<IInstruction>
    //{
    //    InstructionCode Code { get; set; }
    //    StackType StackType { get; set; }
    //    InstructionFlags Flags { get; set; }

    //    FlowControl FlowControl { get; }

    //    int Offset { get; set; }

    //    Register Destination { get; set; }
    //    Register Source1 { get; set; }
    //    Register Source2 { get; set; }
    //    Register Source3 { get; set; }

    //    IInstruction Previous { get; set; }
    //    IInstruction Next { get; set; }

    //    Operand Operand0 { get; set; }
    //    Operand Operand1 { get; set; }
    //    Operand Operand2 { get; set; }

    //    CilInstruction SourceInstruction { get; set; }

    //    IInstruction Add(IInstruction instruction);
    //}

    //public abstract class Instruction : IInstruction
    //{
    //    public IEnumerator<IInstruction> GetEnumerator()
    //    {
    //        yield return this;
    //        var next = this.Next;
    //        if (next == null)
    //            yield break;

    //        foreach (var inst in next)
    //            yield return inst;
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }


    //    public override string ToString()
    //    {
    //        if (this.Code is InstructionCode.Zenos zenos && (zenos.OpCode.IsArgument || zenos.OpCode.IsLocal))
    //        {
    //            return this.Destination.ToString();
    //        }

    //        var sb = new StringBuilder();
    //        sb.AppendFormat("IL_{0:X4}: {1}", this.Offset, this.Code);

    //        sb.Append(" (");
    //        if (this.Source1 != null)
    //        {
    //            sb.Append(this.Source1);
    //            if (this.Source2 != null)
    //                sb.AppendFormat(", {0}", Source2);
    //            if (this.Source3 != null)
    //                sb.AppendFormat(", {0}", Source3);
    //        }
    //        else
    //        {
    //            if (this.Operand0 != null)
    //            {
    //                AppendOperand(sb, this.Operand0);

    //                if (this.Operand1 != null)
    //                {
    //                    sb.Append(", ");
    //                    AppendOperand(sb, this.Operand1);
    //                }
    //            }

    //        }

    //        sb.Append(")");
    //        if (this.Destination != null)
    //            sb.AppendFormat(" => {0}", this.Destination);

    //        return sb.ToString();
    //    }
    //    private void AppendOperand(StringBuilder sb, object operand)
    //    {
    //        var instruction = operand as IInstruction;
    //        if (instruction != null && 
    //            (this.FlowControl == FlowControl.Branch ||
    //             this.FlowControl == FlowControl.Cond_Branch))
    //        {
    //            sb.Append($"IL_{instruction.Offset:X4}");
    //        }
    //        else
    //        {
    //            sb.Append(operand);
    //        }
    //    }

    //    public InstructionCode Code { get; set; }
    //    public StackType StackType { get; set; }
    //    public InstructionFlags Flags { get; set; }
    //    public FlowControl FlowControl { get; set; } = FlowControl.Next;
    //    public int Offset { get; set; }
    //    public Register Destination { get; set; }
    //    public Register Source1 { get; set; }
    //    public Register Source2 { get; set; }
    //    public Register Source3 { get; set; }
    //    public IInstruction Previous { get; set; }
    //    public IInstruction Next { get; set; }
    //    public Operand Operand0 { get; set; }
    //    public Operand Operand1 { get; set; }
    //    public Operand Operand2 { get; set; }
    //    public CilInstruction SourceInstruction { get; set; }

    //    /// <summary>
    //    /// Append instruction to the end of the instruction chain
    //    /// </summary>
    //    /// <param name="instruction"></param>
    //    /// <returns>The last instruction in the chain</returns>
    //    public IInstruction Add(IInstruction instruction)
    //    {
    //        var next = this.Next;
    //        if (next != null)
    //        {
    //            return next.Add(instruction);
    //        }

    //        if (instruction == null)
    //            return this;

    //        this.Next = instruction;
    //        instruction.Previous = this;

    //        return instruction;
    //    }
    //}
}