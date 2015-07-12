using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    //mini.c/mono_codegen
    public class EmitterStage : CodeCompilerStage
    {
        public override void Compile(IMethodContext context)
        {
            var ic = InstructionChain.FromInstructions(context.Instruction);
            
            // prologue
            context.Code.AddRange(new byte[]
            {
                0x55,                   // pushq   %rbp
                0x48, 0x89, 0xe5        // movq    %rsp, %rbp
            });
            // S mod 8 must be 0;
            // S(n) = (n*4)
            var space = SpaceForVariables(context);

            // create some space for our locals and arguments on the stack
            if (space > 0)
            {
                context.Code.AddRange(0x48, 0x83, 0xec, space); // sub    rsp, 0x20
            }

            for (; !ic.EndOfInstructions; ic++)
            {
                var inst = ic.Instruction;
                switch (inst.Code)
                {
                    case InstructionCode.ZilLoad:
                    {
                        if (inst.Operand0 is int)
                        {
                            var i = (int) inst.Operand0;
                            var bytes = BitConverter.GetBytes(i);
                            context.Code.Add(0x68); //push imm32
                            context.Code.AddRange(bytes);
                        }
                        else if (inst.Operand0 is long)
                        {
                            var l = (long)inst.Operand0;
                            var bytes = BitConverter.GetBytes((l));
                            context.Code.AddRange(0x48, 0xb8);  // mov    rax,{l}
                            context.Code.AddRange(bytes);

                            context.Code.AddRange(0x50);        // push   rax
                        }
                        else if (inst.Operand0 is IInstruction)
                        {
                            var reg = (IInstruction) inst.Operand0;
                            var dest = reg.Destination;
                            var offset = (byte)(-4*(dest.Id+1));

                            context.Code.AddRange(0xff, 0x75, offset); //push -4(%rbp)
                        }
                        else
                        {
                            throw new InvalidDataException("Load instruction not supported with operand: " + inst.Operand0);
                        }
                        break;
                    }
                    case InstructionCode.ZilStore:
                    {
                        var reg = (IInstruction)inst.Operand0;
                        var dest = reg.Destination;
                        var offset = (byte)(-4 * (dest.Id + 1));

                        context.Code.AddRange(0x8f, 0x45, offset); //pop -4(%rbp)
                        break;
                    }
                    case InstructionCode.CilNop:
                        break;
                    case InstructionCode.CilRet:
                    {
                        context.Code.AddRange(0x58); //popq    %rax
                        break;
                    }
                    default:
                        throw new InvalidDataException("Unsupported instruction: " + inst.Code);
                }
            }

            // reset the stack pointer to nuke our locals and arguments from the stack
            if (space > 0)
            {
                context.Code.AddRange(0x48, 0x83, 0xc4, space); // add    rsp, 0x20
            }

            // epilogue
            context.Code.AddRange(new byte[]
            {
                0x5d,                   // popq    %rbp
                0xc3,                   // retq
            });

            // add padding to make sure we are byte aligned
            while (true)
            {
                var paddingSize = 16 - (context.Code.Count % 16);
                if (paddingSize == 16)
                    break;

                var nop = paddingSize > 9 ? _paddingBytes[8] : _paddingBytes[paddingSize - 1];
                context.Code.AddRange(nop);
            }
        }

        private int SpaceForVariables(IMethodContext context)
        {
            //Space = (locals + args)*4 bumped to the nearest multiple of 8
            var locals = context.Locals.Count;
            var args = context.Parameters.Count;
            var space = 4*(locals + args);

            //make sure we have a multiple of 8
            space = space%8 == 0 ? space : space + 4;

            return space > 0 && space < 16 ? 16 : space;
        }

        private readonly byte[][] _paddingBytes = 
        {
            new byte[] {0x90}, 
            new byte[] {0x66, 0x90},
            new byte[] {0x0F, 0x1F, 0x00},
            new byte[] {0x0F, 0x1F, 0x40, 0x00},
            new byte[] {0x0F, 0x1F, 0x44, 0x00, 0x00},
            new byte[] {0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00},
            new byte[] {0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00},
        };

        //public override void Compile(IMethodContext context)
        //{
        //    var body = context.Method.Body;

        //    context.OutputFile = body.Method.Name.ToFileName().AppendRandom("_", 32, ".s");

        //    context.Text.WriteLine(".globl _{0}", body.Method.Name);
        //    context.Text.WriteLine(".def	_{0};	.scl	2;	.type	32;	.endef", body.Method.Name);
        //    context.Text.WriteLine("_{0}:", body.Method.Name);

        //    if (body.Instructions.Count == 0)
        //        return;

        //    context.Text.WriteLine("# prologue ");
        //    //save callee stack frame 
        //    context.Text.WriteLine("pushl %ebp          # store the stack frame of the calling function on the stack");
        //    context.Text.WriteLine("movl %esp, %ebp     # takes the current stack pointer and uses it as the frame for the called function");

        //    //add variable space to stack
        //    if (body.HasVariables)
        //        context.Text.WriteLine("subl ${0}, %esp     # make room for local variables ", body.Variables.Count * 4);

        //    context.Text.WriteLine("# body ");

        //    var block = context.bb_entry;
        //    while (block != null)
        //    {
        //        Compile(context, block);
        //        block = block.next_bb;
        //    }
        //    //TODO: base.Compile(context);

        //    context.Text.WriteLine("# epilogue ");

        //    //reset to callee stack frame
        //    context.Text.WriteLine("leave               # restore calling function stack frame");
        //    context.Text.WriteLine("ret");
        //}

        private void Compile(IMethodContext context, BasicBlock block)
        {
            var label = GetBlockLabel(context, block);
            context.Text.WriteLine("{0}:     # {1}", label, block);
            var instruction = block.code;
            while (instruction != null)
            {
                Compile(context, instruction);
                instruction = instruction.Next;
            }
            //Block epilogue?
            context.Text.WriteLine("# end of {0}", label);
            context.Text.WriteLine();
        }

        private static string GetBlockLabel(IMethodContext context, BasicBlock block)
        {
            return string.Format("{0}_bb{1}", context.Method.Name, block.block_num);
        }

        private void Compile(IMethodContext context, IInstruction instruction)
        {
            Trace.WriteLine(instruction);

            switch (instruction.Code)
            {
                //case InstructionCode.OP_ICONST:
                //    context.Text.WriteLine("movl ${0}, %{1}     # {2}", instruction.Operand0, instruction.Destination, instruction);
                //    break;

                //case InstructionCode.OP_MOVE:

                //    break;
                //case Code.Stloc:
                //    context.Text.WriteLine("movl %eax, {0}      # {1} ", EmitLocation(context, instruction), instruction);
                //    break;
                //case Code.Nop:
                //    break;
                //case Code.Ldloc:
                //    var varType = ((VariableReference)(instruction.Operand)).VariableType;
                //    var inst = varType.FullName == "System.Single" ? "flds {0}       # {1} " : "movl {0}, %eax       # {1} ";

                //    context.Text.WriteLine(inst, EmitLocation(context, instruction), instruction);
                //    break;
                //case Code.Ldarg:
                //    context.Text.WriteLine("movl {0}, %eax      # {1} ", EmitLocation(context, instruction), instruction);
                //    break;
                //case Code.Ret:
                //    //ret is handled in the method body
                //    Helper.IsNull(instruction.Next);
                //    break;
                //case Code.Ldc_R4:
                //    var inIEEE754 = BitConverter.ToInt32(BitConverter.GetBytes((float)instruction.Operand), 0);
                //    context.Text.WriteLine("movl $0x{0}, %eax    # {1}", inIEEE754.ToString("x"), instruction);
                //    break;
                default:
                    Helper.NotSupported(string.Format("InstructionCode not supported: {0}", instruction.Code));
                    break;
            }
        }

        private string EmitLocation(ICompilationContext context, Instruction instruction)
        {
            if (instruction.Operand is VariableReference)
            {
                var v = instruction.Operand as VariableReference;
                var index = (v.Index + 1) * -4;
                return string.Format("{0}(%ebp)", index);
            }

            if (instruction.Operand is ParameterReference)
            {
                var v = instruction.Operand as ParameterReference;
                var index = 8 + (v.Index) * 4;
                return string.Format("{0}(%ebp)", index);
            }
            Helper.Break();
            return instruction.Operand.ToString();
        }
    }
}
