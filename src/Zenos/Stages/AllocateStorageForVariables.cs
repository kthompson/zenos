using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.FSharp.Core;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class AllocateStorageForVariables : Compiler<IMethodContext>
    {
        private readonly IArchitecture _architecture;

        public AllocateStorageForVariables(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        public override IMethodContext Compile(IMethodContext context)
        {
            var method = context.Method;

            if (method.ReturnType.FullName != "System.Void")
                context.ReturnType = CreateVariable(context, method.ReturnType, InstructionCode.NewZenos(ZenosOpCode.Argument));

            context.Parameters = new List<Instruction>();
            if (method.HasThis)
                context.Parameters.Add(CreateVariable(context, method.Body.ThisParameter.ParameterType, InstructionCode.NewZenos(ZenosOpCode.Argument)));

            foreach (var parameter in method.Parameters)
                context.Parameters.Add(CreateVariable(context, parameter.ParameterType, InstructionCode.NewZenos(ZenosOpCode.Argument)));

            context.Locals = new List<Instruction>();
            foreach (var variable in method.Body.Variables)
                context.Locals.Add(CreateVariable(context, variable.VariableType, InstructionCode.NewZenos(ZenosOpCode.Local)));

            this._architecture.CreateVariables(context);
            return context;
        }

        private Instruction CreateVariable(IMethodContext context, TypeReference type, InstructionCode op)
        {
            var dreg = context.AllocateDestReg();
            var num = context.Variables.Count;
            var ins = new Instruction(
                op,
                FSharpOption<int>.None,
                FSharpOption<Operand>.Some(Operand.NewInt32(num)),
                FSharpOption<Operand>.Some(Operand.NewTypeReference(type)),
                FSharpOption<Operand>.None,
                FSharpOption<Operand>.Some(Operand.NewRegister(dreg)),
                FSharpOption<Mono.Cecil.Cil.Instruction>.None
            );

                
                //StackType = StackTypeFromType(type)
            //};
            context.Variables.Add (ins);

            return ins;
        }

        private StackType StackTypeFromType(TypeReference type)
        {
            if (type.IsValueType)
            {
                switch (type.Name)
                {
                    case nameof(Boolean):
                    case nameof(Int16):
                    case nameof(UInt16):
                    case nameof(UInt32):
                    case nameof(Int32):
                        return StackType.Imm32;

                    case nameof(Int64):
                    case nameof(UInt64):
                        return StackType.Imm64;

                    case nameof(Double):
                    case nameof(Single):
                        throw new NotImplementedException();

                    default:
                        return StackType.Unknown;
                }
            }
            else
            {
                return StackType.Pointer;
            }
        }
    }
}