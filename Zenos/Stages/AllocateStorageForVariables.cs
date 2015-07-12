using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class AllocateStorageForVariables : CodeCompilerStage
    {
        private readonly IArchitecture _architecture;

        public AllocateStorageForVariables(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        public override void Compile(IMethodContext context)
        {
            var method = context.Method;

            if (method.ReturnType.FullName != "System.Void")
                context.ReturnType = CreateVariable(context, method.ReturnType, InstructionCode.ZilArgument);

            context.Parameters = new List<IInstruction>();
            if (method.HasThis)
                context.Parameters.Add(CreateVariable(context, method.Body.ThisParameter.ParameterType, InstructionCode.ZilArgument));

            foreach (var parameter in method.Parameters)
                context.Parameters.Add(CreateVariable(context, parameter.ParameterType, InstructionCode.ZilArgument));

            context.Locals = new List<IInstruction>();
            foreach (var variable in method.Body.Variables)
                context.Locals.Add(CreateVariable(context, variable.VariableType, InstructionCode.ZilLocal));

            this._architecture.CreateVariables(context);
        }

        private IInstruction CreateVariable(IMethodContext context, TypeReference type, InstructionCode op)
        {
            var dreg = context.AllocateDestReg();

            return CreateVariableForVreg(context, type, op, dreg);
        }

        private IInstruction CreateVariableForVreg(IMethodContext context, TypeReference type, InstructionCode op, IRegister dreg)
        {
			var num = context.Variables.Count;
			var ins = new ZenosInstruction {
				Code = op,
				Operand0 = num,
				Operand1 = type,
				Destination = dreg,
			};
			context.Variables.Add (ins);
			return ins;
        }
    }
}