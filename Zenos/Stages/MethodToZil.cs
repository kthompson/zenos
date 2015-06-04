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

        void CreateVariables(IMethodContext context)
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

            return mono_compile_create_var_for_vreg(context, type, op, dreg);
        }

        private IInstruction mono_compile_create_var_for_vreg(IMethodContext context, TypeReference type, InstructionCode op, IRegister dreg)
        {

            //type = type.mini_replace_type();
            //var num = cfg.Variables.Count;
            //var inst = _emitter.MONO_INST_NEW(cfg, opcode);

            //inst.set_inst_c0(num);
            //inst.set_inst_vtype(type);
            //inst.klass = type.mono_class_from_mono_type();
            //_emitter.type_to_eval_stack_type(cfg, type, inst);
            ////    /* if set to 1 the variable is native */
            ////    inst.backend.is_pinvoke = 0;
            //inst.Destination = vreg;

            ////if (inst.klass.IsExceptionType())
            ////    mono_cfg_set_exception(cfg, MONO_EXCEPTION_TYPE_LOAD);

            ////    if (cfg.compute_gc_maps) {
            ////        if (type.byref) {
            ////            mono_mark_vreg_as_mp (cfg, vreg);
            ////        } else {
            ////            MonoType *t = mini_type_get_underlying_type (NULL, type);
            ////            if ((MONO_TYPE_ISSTRUCT (t) && inst.klass.has_references) || mini_type_is_reference (cfg, t)) {
            ////                inst.flags |= MONO_INST_GC_TRACK;
            ////                mono_mark_vreg_as_ref (cfg, vreg);
            ////            }
            ////        }
            ////    }

            //cfg.Variables.Add(inst);

            //var vi = MONO_INIT_VARINFO(cfg, type);
            //vi.VirtualRegister = vreg;
            
            //if (vreg != null)
            //    set_vreg_to_inst(cfg, vreg, inst);

            ////#ifdef MONO_ARCH_SOFT_FLOAT
            ////bool regpair = type.mono_type_is_long() || type.mono_type_is_float();
            ////#else
            //bool regpair = type.mono_type_is_long();
            ////#endif


            //if (!regpair) 
            //    return inst;

            ///* 
            //         * These two cannot be allocated using create_var_for_vreg since that would
            //         * put it into the cfg.varinfo array, confusing many parts of the JIT.
            //         */

            ///* 
            //         * Set flags to VOLATILE so SSA skips it.
            //         */

            ////if (cfg.verbose_level >= 4) {
            ////    printf ("  Create LVAR R%d (R%d, R%d)\n", inst.dreg, inst.dreg + 1, inst.dreg + 2);
            ////}


            ////if (cfg.opt & MONO_OPT_SSA) {
            ////    if (mono_type_is_float (type))
            ////        inst.flags = MONO_INST_VOLATILE;
            ////}


            ///* Allocate a dummy MonoInst for the first vreg */
            //var tree = _emitter.MONO_INST_NEW(cfg, InstructionCode.OP_LOCAL);
            //tree.Destination = new Register(inst.Destination.Id + 1);
            ////if (cfg.opt & MONO_OPT_SSA)
            ////    tree.flags = MONO_INST_VOLATILE;
            //tree.Operand0 = vi;
            //tree.StackType = StackType.STACK_I4;
            //tree.Operand1 = mono_defaults.int32_class;
            //tree.klass = (mono_defaults.int32_class).mono_class_from_mono_type();

            //set_vreg_to_inst(cfg, tree.Destination, tree);

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