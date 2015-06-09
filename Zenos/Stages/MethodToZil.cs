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
    public class MethodToZil : CodeCompilerStage
    {
        private readonly IArchitecture _architecture;

        public MethodToZil(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        public override void Compile(IMethodContext context)
        {
            CreateVariables(context);
        }

        private void CreateVariables(IMethodContext context)
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
			///* Allocate a dummy MonoInst for the second vreg */
            //tree = _emitter.MONO_INST_NEW(cfg, InstructionCode.OP_LOCAL);
            //tree.Destination = new Register(inst.Destination.Id + 2);
            ////if (cfg.opt & MONO_OPT_SSA)
            ////    tree.flags = MONO_INST_VOLATILE;
            //tree.Operand0 = vi;
            //tree.StackType = StackType.STACK_I4;
            //tree.Operand1 = mono_defaults.int32_class;
            //tree.klass = mono_defaults.int32_class.mono_class_from_mono_type();

            //set_vreg_to_inst(cfg, tree.Destination, tree);

            //return inst;

            throw new NotImplementedException();
        }
    }
}