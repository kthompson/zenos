using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.FSharp.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Zenos.Core;
using Zenos.Framework;
using Instruction = Zenos.Framework.Instruction;

namespace Zenos.Stages
{
    //mini.c/mono_codegen
//    public class EmitterStage : Compiler<IMethodContext>
//    {
//        public override IMethodContext Compile(IMethodContext context)
//        {
//            //// prologue
//            context.Code.AddRange(new byte[]
//            {
//                0x55,                   // pushq   %rbp
//                0x48, 0x89, 0xe5        // movq    %rsp, %rbp
//            });
//            //// S mod 8 must be 0;
//            //// S(n) = (n*4)
//            //var space = SpaceForVariables(context);

//            //// create some space for our locals and arguments on the stack
//            //if (space > 0)
//            //{
//            //    context.Code.AddRange(0x48, 0x83, 0xec, space); // sub    rsp, 0x20
//            //}

//            foreach (var inst in context.Instruction)
//            {
//                switch (inst.Code)
//                {
//                    case InstructionCode.Zenos zenos:
//                    {
//                        var op = zenos.OpCode;
//                        EmitZenosOpCode(context, op, inst);
//                        break;
//                    }
//                    case InstructionCode.Cil cil:
//                    {
//                        EmitCilOpCode(context, cil, inst);
//                        break;
//                    }
//            }

//            //// reset the stack pointer to nuke our locals and arguments from the stack
//            //if (space > 0)
//            //{
//            //    context.Code.AddRange(0x48, 0x83, 0xc4, space); // add    rsp, 0x20
//            //}

//            // epilogue
//            context.Code.AddRange(new byte[]
//            {
//                0x5d,                   // popq    %rbp
//                0xc3,                   // retq
//            });

//            // add padding to make sure we are byte aligned
//            while (true)
//            {
//                var paddingSize = 16 - (context.Code.Count % 16);
//                if (paddingSize == 16)
//                    break;

//                var nop = paddingSize > 9 ? _paddingBytes[8] : _paddingBytes[paddingSize - 1];
//                context.Code.AddRange(nop);
//            }

//            return context;
//        }

//        private void EmitZenosOpCode(IMethodContext context, ZenosOpCode op, Instruction inst)
//        {
//            if (op.IsLoad)
//            {
//                EmitLoad(context, inst);
//            }
//            else if (op.IsStore)
//            {
//                EmitStore(context, inst);
//            }
//            else if (op.IsPushArgument)
//            {
//                EmitPushArgument(context, inst);
//            }
//            else if (op.IsPushConstant)
//            {
//                EmitPushConstanct(context, inst);
//            }
//        }

//        private void EmitPushArgument(IMethodContext context, Instruction inst)
//        {
//            throw new NotImplementedException();
//        }

//        private void EmitPushConstanct(IMethodContext context, Instruction inst)
//        {
//            throw new NotImplementedException();

//            if (inst.Operand0 is float)
//            {
//                context.Code.Add(0x50); // push r32
//            }
//            else
//            {
//            }
//        }

//        private void EmitAdd(IMethodContext context, Instruction inst)
//        {
//            //TODO: basically we need to look at what the stack would have as far as types as we process it
//            // once we know types and what not we can perform actions based on it

//            // context.Code.AddRange(0x8f, 0x45, offset); //pop -4(%rbp)
////            switch (inst.Previous.StackType)
////            {
////                case StackType.Imm64:
////                    // pop stack to eax
////                    // add eax + eax
////                    throw new NotImplementedException();

////                case StackType.Imm32:
////#warning  http://pasm.pis.to/
////#warning                     something is wrong here result is always showing 4. maybe need to run a simulator or something

//                    context.Code.AddRange(new byte[]
//                    {
//                        0x58,             // pop rax
//                        0x41, 0x5A,       // pop r10
//                        0x4C, 0x01, 0xD0, // add rax, rcx
//                        0x50,             // push rax
//                    });

//            //        break;
//            //    case StackType.Pointer:
//            //        throw new NotImplementedException();

//            //    default:
//            //        throw new NotImplementedException();
//            //}
//        }

//        private static void EmitStore(IMethodContext context, Instruction inst)
//        {
//            var reg = (Instruction) inst.Operand0;
//            var dest = reg.Destination;
//            var offset = (byte) (-4*(dest.Id + 1));

//            // TODO: there is more to it than this but for now we will assume that these registers are int/pointer types
//            switch (dest.Id)
//            {
//                case 0: 
//                    context.Code.Add(0x59); // pop rcx
//                    break;

//                case 1: 
//                    context.Code.Add(0x5A); // pop rdx
//                    break;

//                case 2: 
//                    context.Code.AddRange(0x41, 0x58); // pop r8
//                    break;

//                case 3: 
//                    context.Code.AddRange(0x41, 0x59);// pop r9
//                    break;

//                default:
//                    context.Code.AddRange(0x8f, 0x45, offset); //pop qword [rbp-4]
//                    break;
//            }
//        }

//        private static void EmitLoad(IMethodContext context, Instruction inst)
//        {
//            if (FSharpOption<Operand>.get_IsNone(inst.Operand0))
//                return;

//            switch (inst.Operand0.Value)
//            {
//                case Operand.Int32 intOp:
//                {
//                    var i = intOp.Item;
//                    var bytes = BitConverter.GetBytes(i);
//                    context.Code.Add(0x68); //push imm32
//                    context.Code.AddRange(bytes);
//                    break;

//                }
//                case Operand.Int64 intOp:
//                {
//                    var l = intOp.Item;
//                    var bytes = BitConverter.GetBytes((l));
//                    context.Code.AddRange(0x48, 0xb8); // mov rax,{l}
//                    context.Code.AddRange(bytes);

//                    context.Code.AddRange(0x50); // push   rax
//                    break;

//                }
//                case Operand.Instruction instOp: // argument
//                {
//                    var reg = instOp.Item;
//                    var dest = reg.Destination;
//                    var offset = (byte) (-4 * (dest.Id + 1));

//                    // TODO: there is more to it than this but for now we will assume that these registers are int/pointer types
//                    switch (dest.Id)
//                    {
//                        case 0:
//                            context.Code.Add(0x51); // push rcx
//                            break;

//                        case 1:
//                            context.Code.Add(0x52); // push rdx
//                            break;

//                        case 2:
//                            context.Code.AddRange(0x41, 0x50); // push r8
//                            break;

//                        case 3:
//                            context.Code.AddRange(0x41, 0x51); // push r9
//                            break;

//                        default:
//                            context.Code.AddRange(0xff, 0x75, offset); //push qword [rbp-4]
//                            break;
//                    }
//                    break;
//                }
//                default:
//                {
//                    throw new InvalidDataException("Load instruction not supported with operand: " + inst.Operand0);
//                }
//            }
//        }

//        private int SpaceForVariables(IMethodContext context)
//        {
//            //TODO: this should be calculated based on the stack type for each parameter and argument
            
//            //Space = (locals + args)*4 bumped to the nearest multiple of 8
//            var locals = 0; //context.Locals.Count;
//            var args = context.Parameters.Count;
//            var space = 4*(locals + args);

//            //make sure we have a multiple of 8
//            space = space%8 == 0 ? space : space + 4;

//            return space > 0 && space < 16 ? 16 : space;
//        }

//        private static void EmitCilOpCode(IMethodContext context, InstructionCode.Cil cil, object inst)
//        {
//            var op = cil.OpCode;
//            switch (op)
//            {
//                case Code.Nop:
//                    break;

//                case Code.Ret:
//                {
//                    context.Code.AddRange(0x58); // popq    %rax
//                    break;
//                }
//                case Code.Add:
//                {
//                    EmitAdd(context, inst);
//                    break;
//                }
//                case Code.Ldarg_0:
//                {
//                    context.Code.Add(0x51); // push rcx
//                    break;
//                }
//                case Code.Ldarg_1:
//                {
//                    context.Code.Add(0x52); // push rdx
//                    break;
//                }
//                case Code.Ldarg_2:
//                {
//                    context.Code.AddRange(0x41, 0x50); // push r8
//                    break;
//                }
//                case Code.Ldarg_3:
//                {
//                    context.Code.AddRange(0x41, 0x51); // push r9
//                    break;
//                }
//                default:
//                    throw new InvalidDataException("Unsupported instruction: " + inst.Code);
//            }
//        }

//        private readonly byte[][] _paddingBytes = 
//        {
//            new byte[] {0x90}, 
//            new byte[] {0x66, 0x90},
//            new byte[] {0x0F, 0x1F, 0x00},
//            new byte[] {0x0F, 0x1F, 0x40, 0x00},
//            new byte[] {0x0F, 0x1F, 0x44, 0x00, 0x00},
//            new byte[] {0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00},
//            new byte[] {0x0F, 0x1F, 0x80, 0x00, 0x00, 0x00, 0x00},
//            new byte[] {0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00},
//            new byte[] {0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00},
//        };

//        //public override void Compile(IMethodContext context)
//        //{
//        //    var body = context.Method.Body;

//        //    context.OutputFile = body.Method.Name.ToFileName().AppendRandom("_", 32, ".s");

//        //    context.Text.WriteLine(".globl _{0}", body.Method.Name);
//        //    context.Text.WriteLine(".def	_{0};	.scl	2;	.type	32;	.endef", body.Method.Name);
//        //    context.Text.WriteLine("_{0}:", body.Method.Name);

//        //    if (body.Instructions.Count == 0)
//        //        return;

//        //    context.Text.WriteLine("# prologue ");
//        //    //save callee stack frame 
//        //    context.Text.WriteLine("pushl %ebp          # store the stack frame of the calling function on the stack");
//        //    context.Text.WriteLine("movl %esp, %ebp     # takes the current stack pointer and uses it as the frame for the called function");

//        //    //add variable space to stack
//        //    if (body.HasVariables)
//        //        context.Text.WriteLine("subl ${0}, %esp     # make room for local variables ", body.Variables.Count * 4);

//        //    context.Text.WriteLine("# body ");

//        //    var block = context.bb_entry;
//        //    while (block != null)
//        //    {
//        //        Compile(context, block);
//        //        block = block.next_bb;
//        //    }
//        //    //TODO: base.Compile(context);

//        //    context.Text.WriteLine("# epilogue ");

//        //    //reset to callee stack frame
//        //    context.Text.WriteLine("leave               # restore calling function stack frame");
//        //    context.Text.WriteLine("ret");
//        //}

//        //private void Compile(IMethodContext context, BasicBlock block)
//        //{
//        //    var label = GetBlockLabel(context, block);
//        //    context.Text.WriteLine("{0}:     # {1}", label, block);
//        //    var instruction = block.Instruction;
//        //    while (instruction != null)
//        //    {
//        //        Compile(context, instruction);
//        //        instruction = instruction.Next;
//        //    }
//        //    //Block epilogue?
//        //    context.Text.WriteLine("# end of {0}", label);
//        //    context.Text.WriteLine();
//        //}

//        //private static string GetBlockLabel(IMethodContext context, BasicBlock block)
//        //{
//        //    return $"{context.Method.Name}_bb{block.BlockId}";
//        //}

//        //private void Compile(IMethodContext context, IInstruction instruction)
//        //{
//        //    Trace.WriteLine(instruction);

//        //    switch (instruction.Code)
//        //    {
//        //        //case InstructionCode.OP_ICONST:
//        //        //    context.Text.WriteLine("movl ${0}, %{1}     # {2}", instruction.Operand0, instruction.Destination, instruction);
//        //        //    break;

//        //        //case InstructionCode.OP_MOVE:

//        //        //    break;
//        //        //case Code.Stloc:
//        //        //    context.Text.WriteLine("movl %eax, {0}      # {1} ", EmitLocation(context, instruction), instruction);
//        //        //    break;
//        //        //case Code.Nop:
//        //        //    break;
//        //        //case Code.Ldloc:
//        //        //    var varType = ((VariableReference)(instruction.Operand)).VariableType;
//        //        //    var inst = varType.FullName == "System.Single" ? "flds {0}       # {1} " : "movl {0}, %eax       # {1} ";

//        //        //    context.Text.WriteLine(inst, EmitLocation(context, instruction), instruction);
//        //        //    break;
//        //        //case Code.Ldarg:
//        //        //    context.Text.WriteLine("movl {0}, %eax      # {1} ", EmitLocation(context, instruction), instruction);
//        //        //    break;
//        //        //case Code.Ret:
//        //        //    //ret is handled in the method body
//        //        //    Helper.IsNull(instruction.Next);
//        //        //    break;
//        //        //case Code.Ldc_R4:
//        //        //    var inIEEE754 = BitConverter.ToInt32(BitConverter.GetBytes((float)instruction.Operand), 0);
//        //        //    context.Text.WriteLine("movl $0x{0}, %eax    # {1}", inIEEE754.ToString("x"), instruction);
//        //        //    break;
//        //        default:
//        //            Helper.NotSupported($"InstructionCode not supported: {instruction.Code}");
//        //            break;
//        //    }
//        //}

//        //private string EmitLocation(ICompilationContext context, Instruction instruction)
//        //{
//        //    var operand = instruction.Operand as VariableReference;
//        //    if (operand != null)
//        //    {
//        //        var index = (operand.Index + 1) * -4;
//        //        return $"{index}(%ebp)";
//        //    }

//        //    var parameter = instruction.Operand as ParameterReference;
//        //    if (parameter != null)
//        //    {
//        //        var index = 8 + (parameter.Index) * 4;
//        //        return $"{index}(%ebp)";
//        //    }
//        //    Helper.Break();
//        //    return instruction.Operand.ToString();
//        //}
//    }

    /// <summary>
    /// Mode Registry Memory (aka ModRM)
    /// </summary>
    enum ModeRegistryMemory : byte
    {
        RM0 = 0x01,
        RM1 = 0x02,
        RM2 = 0x04,
        RegOpCode0 = 0x08,
        RegOpCode1 = 0x10,
        RegOpCode2 = 0x20,
        Mod0 = 0x40,
        Mod1 = 0x80,
    }

    /// <summary>
    /// Scale Index Base (SIB)
    /// 
    /// * scale[7:6]: 2[6:7]scale = scale factor
    /// * index[.X, 5:3] – reg containing the index portion
    /// * base[.B, 2:0] – reg containing the base portion
    /// * eff_addr = scale* index + base + offset
    /// </summary>
    enum ScaleIndexBase : byte
    {
        Base0 = 0x01,
        Base1 = 0x02,
        Base2 = 0x04,
        Index0 = 0x08,
        Index1 = 0x10,
        Index2 = 0x20,
        Scale0 = 0x40,
        Scale1 = 0x80,
    }
}
