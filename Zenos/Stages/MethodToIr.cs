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
    public class MethodToIr : CodeCompilerStage
    {
        private readonly IArchitecture _architecture;
        private readonly Emitter _emitter;

        public MethodToIr(IArchitecture architecture, Emitter emitter)
        {
            _architecture = architecture;
            _emitter = emitter;
        }

        public override void Compile(IMethodContext context)
        {

            bool init_locals;
            
            //if(unsafe code)
            //    init_locals = context.Method.Body.InitLocals;
            init_locals = true;

            var start_new_bblock = 0;
            BasicBlock tblock = null;

            /* ENTRY BLOCK */
            var start_bblock = NEW_BBLOCK(context);
            context.bb_entry = start_bblock;

            /* EXIT BLOCK */
            var end_bblock = NEW_BBLOCK(context);
            context.bb_exit = end_bblock;
            end_bblock.Flags |= BasicBlockFlags.BB_INDIRECT_JUMP_TARGET;
            
            /* FIRST CODE BLOCK */
            var bblock = NEW_BBLOCK(context);
            bblock.cil_code = context.Instruction;
            context.CurrentBasicBlock = bblock;
            //cfg->ip = ip;

            ADD_BBLOCK(context, bblock);


            /* we use a separate basic block for the initialization code */
            var init_localsbb = NEW_BBLOCK(context);
            context.bb_init = init_localsbb;
            init_localsbb.real_offset = context.real_offset;
            start_bblock.next_bb = init_localsbb;
            init_localsbb.next_bb = bblock;
            link_bblock(context, start_bblock, init_localsbb);
            link_bblock(context, init_localsbb, bblock);

            context.CurrentBasicBlock = init_localsbb;

            CreateVars(context);


            var sp = new InstructionChain(); 
            var ip = InstructionChain.FromInstructions(context.Instruction);

            context.CurrentBasicBlock = bblock;
            
            while (ip.Instruction != null)
            {
                context.Instruction = ip.Instruction;
                if (start_new_bblock != 0)
                {
                    bblock.cil_length = ip.Instruction.Offset - bblock.cil_code.Offset;
                    if (start_new_bblock == 2)
                    {
                        //g_assert(instruction.Offset == tblock->cil_code);
                    }
                    else
                    {
                        tblock = GET_BBLOCK(context, ip.Instruction);
                    }
                    bblock.next_bb = tblock;
                    bblock = tblock;
                    context.CurrentBasicBlock = bblock;
                    start_new_bblock = 0;
                    for (var i = 0; i < bblock.in_stack.Count; ++i)
                    {
                        //if (context.verbose_level > 3)
                        //    printf("loading %d from temp %d\n", i, (int)bblock.in_stack[i]->inst_c0);
                        var ins = _emitter.EMIT_NEW_TEMPLOAD(context, bblock.in_stack[i].inst_c0());
                        sp.AssignAndIncrement(ins);
                    }
                    //if (class_inits)
                    //    g_slist_free(class_inits);
                    //class_inits = NULL;
                }
                else
                {
                    if ((tblock = context.cil_offset_to_bb.GetOrDefault(ip.Offset - context.cil_start)) != null && (tblock != bblock))
                    {
                        link_bblock(context, bblock, tblock);
                        if (sp.Index != 0)
                        {
                            Helper.Break("this should be the first instruction where the null is but its not implemented anyways");
                            handle_stack_args(context, null /* stack_start */, sp.Offset);
                            //handle_stack_args(context, stack_start, sp.Offset);
                            sp.Reset();;
                            //CHECK_UNVERIFIABLE(context);
                        }
                        bblock.next_bb = tblock;
                        bblock = tblock;
                        context.CurrentBasicBlock = bblock;
                        for (var i = 0; i < bblock.in_stack.Count; ++i)
                        {
                            //if (context.verbose_level > 3)
                            //    printf("loading %d from temp %d\n", i, (int)bblock.in_stack[i]->inst_c0);
                            var ins = _emitter.EMIT_NEW_TEMPLOAD(context, bblock.in_stack[i].inst_c0());
                            sp.AssignAndIncrement(ins);
                        }
                        //g_slist_free(class_inits);
                        //class_inits = NULL;
                    }
                }

                switch (ip.Instruction.Code)
                {
                    case InstructionCode.CilLdc_I4_0:
                    case InstructionCode.CilLdc_I4_1:
                    case InstructionCode.CilLdc_I4_2:
                    case InstructionCode.CilLdc_I4_3:
                    case InstructionCode.CilLdc_I4_4:
                    case InstructionCode.CilLdc_I4_5:
                    case InstructionCode.CilLdc_I4_6:
                    case InstructionCode.CilLdc_I4_7:
                    case InstructionCode.CilLdc_I4_8:
                    {
                        CHECK_STACK_OVF(context, 1);
                        var ins = _emitter.EMIT_NEW_ICONST(context, (ip.Code) - InstructionCode.CilLdc_I4_0);
                        ip.Increment();
                        sp.AssignAndIncrement(ins);
                        break;
                    }
                    case InstructionCode.CilLdarg_0:
                    case InstructionCode.CilLdarg_1:
                    case InstructionCode.CilLdarg_2:
                    case InstructionCode.CilLdarg_3:
                    {
                        CHECK_STACK_OVF(context, 1);
                        var n = (ip.Code) - InstructionCode.CilLdarg_0;
                        CHECK_ARG(context, n);
                        var ins = _emitter.EMIT_NEW_LOAD_ARG(context, n);
                        ip.Increment();
                        sp.AssignAndIncrement(ins);
                        break;
                    }
                    case InstructionCode.CilLdloc_0:
                    case InstructionCode.CilLdloc_1:
                    case InstructionCode.CilLdloc_2:
                    case InstructionCode.CilLdloc_3:
                    {
                        CHECK_STACK_OVF(context, 1);
                        var n = (ip.Code) - InstructionCode.CilLdloc_0;
                        CHECK_LOCAL(context, n);
                        var ins = _emitter.EMIT_NEW_LOCLOAD(context, n);
                        ip.Increment();
                        sp.AssignAndIncrement(ins);
                        break;
                    }

                    case InstructionCode.CilStloc_0:
                    case InstructionCode.CilStloc_1:
                    case InstructionCode.CilStloc_2:
                    case InstructionCode.CilStloc_3:
                    {
                        CHECK_STACK(context, 1);
                        var n = (ip.Code) - InstructionCode.CilStloc_0;
                        CHECK_LOCAL(context, n);

                        /* basically we need to look at the previous instr to see if we can do a small optimiation
                         * once we do it we need to skip the current instr(CilStloc) and continue with the one after
                         */
                        sp.Decrement();
                        //if (!dont_verify_stloc && target_type_is_incompatible(cfg, header.locals[n], *sp))
                        //    UNVERIFIED;
                        emit_stloc_ir(context, sp.Instruction, n);
                        ip.Increment();
                        //inline_costs += 1;
                        break;
                    }

                    case InstructionCode.CilRet:
                    {

                        if (context.ReturnType != null)
                        {
                            var ret_type = context.Method.ReturnType;

                            //if (seq_points && !sym_seq_points) {
                            //    /* 
                            //     * Place a seq point here too even through the IL stack is not
                            //     * empty, so a step over on
                            //     * call <FOO>
                            //     * ret
                            //     * will work correctly.
                            //     */
                            //    NEW_SEQ_POINT (cfg, ins, ip - header.code, TRUE);
                            //    MONO_ADD_INS (cfg.cbb, ins);
                            //}

                            //g_assert (!return_var);
                            CHECK_STACK(context, 1);

                            sp.Decrement();

                            //if ((method.wrapper_type == MONO_WRAPPER_DYNAMIC_METHOD || method.wrapper_type == MONO_WRAPPER_NONE) && target_type_is_incompatible (cfg, ret_type, *sp))
                            //    UNVERIFIED;

                            if (mini_type_to_stind(context, ret_type) == InstructionCode.CilStobj)
                            {
                                if (context.vret_addr == null)
                                {
                                    var ins = _emitter.EMIT_NEW_VARSTORE(context, context.ReturnType, ret_type, sp.Instruction);
                                    sp.Instruction = ins;
                                }
                                else
                                {
                                    var ret_addr = _emitter.EMIT_NEW_RETLOADA(context);

                                    var ins = _emitter.EMIT_NEW_STORE_MEMBASE(context, InstructionCode.OP_STOREV_MEMBASE,
                                        ret_addr.Destination, 0, ip.Destination);

                                    ins.klass = ret_type.mono_class_from_mono_type();
                                }
                            }
                            else
                            {
                                _architecture.mono_arch_emit_setret(context, context.Method, sp.Instruction);
                            }
                        }

                        var inst = _emitter.MONO_INST_NEW(context, InstructionCode.OP_BR);

                        ip.Increment();
                        inst.set_inst_target_bb(end_bblock);
                        _emitter.MONO_ADD_INS(bblock, inst);
                        link_bblock(context, bblock, end_bblock);
                        start_new_bblock = 1;

                        break;
                    }
                    default:
                        Helper.Stop("Unsupported instruction: " + ip.Code);
                        break;
                }
            }

            if (bblock.next_bb != null)
            {
                /* This could already be set because of inlining, #693905 */
                var bb = bblock;

                while (bb.next_bb != null)
                    bb = bb.next_bb;
                
                bb.next_bb = end_bblock;
            }
            else
            {
                bblock.next_bb = end_bblock;
            }

            Helper.Break();

            if (init_localsbb != null)
            {
                context.CurrentBasicBlock = init_localsbb;
                context.Instruction = null;
                for (int i = 0; i < context.VariableDefinitions.Count; i++)
                {
                    _emitter.emit_init_local(context, i, context.VariableDefinitions[i].VariableType, init_locals);
                }
            }

            //TODO: emit ref vars


        }


        private void handle_stack_args(IMethodContext context, IInstruction stackStart, int count)
        {
            throw new NotImplementedException();
        }

        //mono_method_to_ir

        private void link_bblock(IMethodContext cfg, BasicBlock from, BasicBlock to)
        {
            if (!from.out_bb.Contains(to))
                from.out_bb.Add(to);

            if (!to.in_bb.Contains(from))
                to.in_bb.Add(from);
        }

        private void CHECK_STACK_OVF(IMethodContext context, int n)
        {
            //if (((sp - stack_start) + n) > header.max_stack) UNVERIFIED();
        }

        private BasicBlock NEW_BBLOCK(IMethodContext cfg)
        {
            return new BasicBlock
            {
                block_num = cfg.num_bblocks++
            };
        }

        private void ADD_BBLOCK(IMethodContext cfg, BasicBlock b)
        {
            if (b.cil_code != null)
            {
                cfg.cil_offset_to_bb[b.cil_code.Offset - cfg.cil_start] = b;	
            } 
            b.real_offset = cfg.real_offset;	
        }

        private BasicBlock GET_BBLOCK(IMethodContext cfg, IInstruction ip)
        {
            var tblock = cfg.cil_offset_to_bb.GetOrDefault(ip.Offset - cfg.cil_start);

            if (tblock != null)
                return tblock;
            //if ((ip) >= end || (ip) < header->code) 
            //    UNVERIFIED;

            tblock = NEW_BBLOCK(cfg);
            tblock.cil_code = ip;
            ADD_BBLOCK(cfg, tblock);

            return tblock;
        }

        private void CHECK_LOCAL(IMethodContext context, int num)
        {
            if (num >= context.Variables.Count)
                UNVERIFIED();
        }


        private void CHECK_ARG(IMethodContext context, int num)
        {
            if (num >= context.Parameters.Count)
                UNVERIFIED();
        }

        private void UNVERIFIED()
        {
            throw new NotImplementedException();
        }

        private void mono_arch_emit_setret(IMethodContext context, MethodDefinition method, IInstruction instruction)
        {
            throw new NotImplementedException();
        }

        private InstructionCode mini_type_to_stind(IMethodContext cfg, TypeReference type)
        {
            //if (cfg.generic_sharing_context && !type.IsByReference)
            //{
            //    if (type.type == MONO_TYPE_VAR || type.type == MONO_TYPE_MVAR)
            //    {
            //        if (mini_type_var_is_vt(cfg, type))
            //            return CEE_STOBJ;
            //        else
            //            return CEE_STIND_REF;
            //    }
            //}
            return mono_type_to_stind(type);
        }

        private InstructionCode mono_type_to_stind(TypeReference type)
        {
            if(type.IsByReference)
                return InstructionCode.CilStind_I;
            
        handle_enum:
            switch (type.MetadataType)
            {
                case MetadataType.SByte:
                case MetadataType.Byte:
                case MetadataType.Boolean:
                    return InstructionCode.CilStind_I1;
                case MetadataType.Int16:
                case MetadataType.UInt16:
                case MetadataType.Char:
                    return InstructionCode.CilStind_I2;
                case MetadataType.Int32:
                case MetadataType.UInt32:
                    return InstructionCode.CilStind_I4;
                case MetadataType.IntPtr:
                case MetadataType.UIntPtr:
                case MetadataType.Pointer:
                case MetadataType.FunctionPointer:
                    return InstructionCode.CilStind_I;
                case MetadataType.Class:
                case MetadataType.String:
                case MetadataType.Object:
                case (MetadataType)0x1d: //MONO_TYPE_SZARRAY:
                case MetadataType.Array:
                    return InstructionCode.CilStind_Ref;
                case MetadataType.Int64:
                case MetadataType.UInt64:
                    return InstructionCode.CilStind_I8;
                case MetadataType.Single:
                    return InstructionCode.CilStind_R4;
                case MetadataType.Double:
                    return InstructionCode.CilStind_R8;
                case MetadataType.ValueType:
                {
                    var definition = type.Resolve();
                    if (definition.IsEnum)
                    {
                        type = definition.GetEnumUnderlyingType();
                        goto handle_enum;
                    }
                    return InstructionCode.CilStobj;
                }
                case MetadataType.TypedByReference:
                    return InstructionCode.CilStobj;
                case MetadataType.GenericInstance:
                {
                    var definition = type.Resolve();
                    //TODO: type = type.data.generic_class.container_class.byval_arg;
                    goto default;
                    goto handle_enum;
                }
                default:
                    throw new InvalidOperationException(string.Format("unknown type {0} in type_to_stind", type.MetadataType));

            }
        }

        private void emit_stloc_ir(IMethodContext cfg, IInstruction sp, int n)
        {
            var opcode = _emitter.mono_type_to_regmove(cfg, cfg.Locals[n].GetVariableType());
            if (opcode == InstructionCode.OP_MOVE && 
                cfg.CurrentBasicBlock.last_ins == sp &&
                (sp.Code == InstructionCode.OP_ICONST || sp.Code == InstructionCode.OP_I8CONST))
            {
                /* Optimize reg-reg moves away */
                /* 
                 * Can't optimize other opcodes, since sp[0] might point to
                 * the last ins of a decomposed opcode.
                 */
                sp.Destination = cfg.Locals[n].Destination;
            }
            else
            {
                var ins = _emitter.EMIT_NEW_LOCSTORE(cfg, n, sp);
                //TODO: should this have sp.ReplaceWith(ins)?
                Helper.Break();
            }
        }


        private void CHECK_STACK(IMethodContext context, int num)
        {
            //TODO: basically we need to verify if there are at least "num" previous instructions
            
            //if ((sp - stack_start) < (num)) UNVERIFIED();
        }


        //mono_compile_create_vars
        private void CreateVars(IMethodContext context)
        {
            var method = context.Method;

            if (method.ReturnType.FullName != "System.Void")
                context.ReturnType = mono_compile_create_var(context, method.ReturnType, InstructionCode.OP_ARG);

            context.Parameters = new List<IInstruction>();
            if (method.HasThis)
                context.Parameters.Add(mono_compile_create_var(context, method.Body.ThisParameter.ParameterType, InstructionCode.OP_ARG));

            foreach (var parameter in method.Parameters)
                context.Parameters.Add(mono_compile_create_var(context, parameter.ParameterType, InstructionCode.OP_ARG));

            context.Locals = new List<IInstruction>();
            foreach (var variable in method.Body.Variables)
                context.Locals.Add(mono_compile_create_var(context, variable.VariableType, InstructionCode.OP_LOCAL));

            mono_arch_create_vars(context);

            //if (context.Method..save_lmf && context.create_lmf_var)
            //{
            //    var lmf_var = mono_compile_create_var(context, !mono_defaults.int_class.IsByReference, InstructionCode.OP_LOCAL);
            //    lmf_var.Flags |= InstructionFlags.Volatile;
            //    lmf_var.Flags |= InstructionFlags.Lmf;
            //    context.lmf_var = lmf_var;
            //}
        }

        private void mono_arch_create_vars(IMethodContext cfg)
        {
            //MonoType* sig_ret;
            //MonoMethodSignature* sig;
            //CallInfo* cinfo;

            //var sig = mono_method_signature(cfg.Method);

            var cinfo = _architecture.get_call_info(cfg, cfg.Method);
            var sig_ret = cfg.Method.ReturnType.mini_replace_type();
            
            if (cinfo.ret.storage == ArgStorage.ArgValuetypeInReg)
                cfg.ret_var_is_local = true;
            if ((cinfo.ret.storage != ArgStorage.ArgValuetypeInReg) && (mono_type_is_struct(sig_ret.Resolve()) || mini_is_gsharedvt_variable_type(cfg, sig_ret)))
            {
                cfg.vret_addr = mono_compile_create_var(cfg, mono_defaults.int_class,  InstructionCode.OP_ARG);
            }


//    if (cfg.method.save_lmf) {
//        cfg.create_lmf_var = TRUE;
//        cfg.lmf_ir = TRUE;
//#ifndef HOST_WIN32
//        cfg.lmf_ir_mono_lmf = TRUE;
//#endif
//    }

            cfg.arch_eh_jit_info = 1;
        }

        private bool mini_is_gsharedvt_variable_type(IMethodContext cfg, TypeReference type)
        {
            return false;
        }

        private bool mono_type_is_struct(TypeDefinition type)
        {
            return (!type.IsByReference && 
                    (
                        (type.MetadataType == MetadataType.ValueType && !type.IsEnum) || 
                        (type.MetadataType == MetadataType.TypedByReference) ||
                        (
                            (type.MetadataType == MetadataType.GenericInstance) &&
                            mono_metadata_generic_class_is_valuetype(type) &&
                            !type.IsEnum)));
        }

        private bool mono_metadata_generic_class_is_valuetype(object genericClass)
        {
            throw new NotImplementedException();
        }

        private IInstruction mono_compile_create_var(IMethodContext context, TypeReference type, InstructionCode op)
        {
            IRegister dreg;

            if (type.FullName == "System.Int64")
                dreg = context.alloc_dreg(StackType.STACK_I8);
            else
                /* All the others are unified */
                dreg = context.alloc_preg();

            return mono_compile_create_var_for_vreg(context, type, op, dreg);
        }


        private IInstruction mono_compile_create_var_for_vreg(IMethodContext cfg, TypeReference type, InstructionCode opcode, IRegister vreg)
        {
            type = type.mini_replace_type();
            var num = cfg.Variables.Count;
            var inst = _emitter.MONO_INST_NEW(cfg, opcode);

            inst.set_inst_c0(num);
            inst.set_inst_vtype(type);
            inst.klass = type.mono_class_from_mono_type();
            _emitter.type_to_eval_stack_type(cfg, type, inst);
            //    /* if set to 1 the variable is native */
            //    inst.backend.is_pinvoke = 0;
            inst.Destination = vreg;

            //if (inst.klass.IsExceptionType())
            //    mono_cfg_set_exception(cfg, MONO_EXCEPTION_TYPE_LOAD);

            //    if (cfg.compute_gc_maps) {
            //        if (type.byref) {
            //            mono_mark_vreg_as_mp (cfg, vreg);
            //        } else {
            //            MonoType *t = mini_type_get_underlying_type (NULL, type);
            //            if ((MONO_TYPE_ISSTRUCT (t) && inst.klass.has_references) || mini_type_is_reference (cfg, t)) {
            //                inst.flags |= MONO_INST_GC_TRACK;
            //                mono_mark_vreg_as_ref (cfg, vreg);
            //            }
            //        }
            //    }

            cfg.Variables.Add(inst);

            var vi = MONO_INIT_VARINFO(cfg, type);
            vi.VirtualRegister = vreg;
            
            if (vreg != null)
                set_vreg_to_inst(cfg, vreg, inst);

            //#ifdef MONO_ARCH_SOFT_FLOAT
            //bool regpair = type.mono_type_is_long() || type.mono_type_is_float();
            //#else
            bool regpair = type.mono_type_is_long();
            //#endif


            if (!regpair) 
                return inst;

            /* 
                     * These two cannot be allocated using create_var_for_vreg since that would
                     * put it into the cfg.varinfo array, confusing many parts of the JIT.
                     */

            /* 
                     * Set flags to VOLATILE so SSA skips it.
                     */

            //if (cfg.verbose_level >= 4) {
            //    printf ("  Create LVAR R%d (R%d, R%d)\n", inst.dreg, inst.dreg + 1, inst.dreg + 2);
            //}


            //if (cfg.opt & MONO_OPT_SSA) {
            //    if (mono_type_is_float (type))
            //        inst.flags = MONO_INST_VOLATILE;
            //}


            /* Allocate a dummy MonoInst for the first vreg */
            var tree = _emitter.MONO_INST_NEW(cfg, InstructionCode.OP_LOCAL);
            tree.Destination = new Register(inst.Destination.Id + 1);
            //if (cfg.opt & MONO_OPT_SSA)
            //    tree.flags = MONO_INST_VOLATILE;
            tree.Operand0 = vi;
            tree.StackType = StackType.STACK_I4;
            tree.Operand1 = mono_defaults.int32_class;
            tree.klass = (mono_defaults.int32_class).mono_class_from_mono_type();

            set_vreg_to_inst(cfg, tree.Destination, tree);

            /* Allocate a dummy MonoInst for the second vreg */
            tree = _emitter.MONO_INST_NEW(cfg, InstructionCode.OP_LOCAL);
            tree.Destination = new Register(inst.Destination.Id + 2);
            //if (cfg.opt & MONO_OPT_SSA)
            //    tree.flags = MONO_INST_VOLATILE;
            tree.Operand0 = vi;
            tree.StackType = StackType.STACK_I4;
            tree.Operand1 = mono_defaults.int32_class;
            tree.klass = mono_defaults.int32_class.mono_class_from_mono_type();

            set_vreg_to_inst(cfg, tree.Destination, tree);

            return inst;
        }

        private void set_vreg_to_inst(IMethodContext cfg, IRegister vreg, IInstruction inst)
        {
            cfg.VRegisterToInstruction[vreg] = inst;
        }

        private IVariableDefinition MONO_VARINFO(IMethodContext cfg, int varnum)
        {
            return cfg.VariableDefinitions[varnum];
        }

        private IVariableDefinition MONO_INIT_VARINFO(IMethodContext cfg, TypeReference type)
        {
            var vi = new VariableDefinition
            {
                Index = cfg.VariableDefinitions.Count,
                VariableType = type,
            };
            cfg.VariableDefinitions.Add(vi);
            //vi.range.first_use.pos.bid = 0xffff; 
            return vi;
        }
    }

    public class CallInfo
    {
        public int nargs;
        public uint stack_usage;
        public uint reg_usage;
        public uint freg_usage;
        public bool need_stack_align;
        public uint stack_align_amount;
        public bool vtype_retaddr;
        /* The index of the vret arg in the argument list */
        public int vret_arg_index;
        public uint vret_arg_offset;
        /* Argument space popped by the callee */
        public int callee_stack_pop;

        public ArgInfo ret;
        public ArgInfo sig_cookie;
        public ArgInfo[] args;
    }

    public class ArgInfo
    {
        public uint offset;
        public IRegister reg;
        public ArgStorage storage;
        public int nslots;
        public bool is_pair;

        /* Only if storage == ArgValuetypeInReg */
        public ArgStorage[] pair_storage = new ArgStorage[2];
        public byte[] pair_regs  = new byte[2];
    }

    public enum ArgStorage
    {
        ArgInIReg,
        ArgInFloatSSEReg,
        ArgInDoubleSSEReg,
        ArgOnStack,
        ArgValuetypeInReg,
        ArgOnFloatFpStack,
        ArgOnDoubleFpStack,
        /* gsharedvt argument passed by addr */
        ArgGSharedVt,
        ArgNone
    }

    public enum X86_Reg_No
    {
        X86_EAX = 0,
        X86_ECX = 1,
        X86_EDX = 2,
        X86_EBX = 3,
        X86_ESP = 4,
        X86_EBP = 5,
        X86_ESI = 6,
        X86_EDI = 7,
        X86_NREG
    };


    static class TypeDefinitionExtensions
    {
        public static TypeDefinition mono_type_get_underlying_type(this TypeDefinition type)
        {
            if (type.IsValueType && type.IsEnum && !type.IsByReference)
                return type.GetEnumUnderlyingType();

            //TODO:  investigate further if (type.IsGenericInstance && type.data.generic_class.container_class.enumtype && !type.IsByReference)
            //    return GetEnumUnderlyingType(type.data.generic_class.container_class);
            return type;
        }


        public static TypeDefinition GetEnumUnderlyingType(this TypeDefinition self)
        {
            var fields = self.Fields;

            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (!field.IsStatic)
                    return field.FieldType.Resolve();
            }

            throw new ArgumentException();
        }


        public static bool MONO_TYPE_IS_REFERENCE(this TypeReference retType)
        {
            throw new NotImplementedException();
        }

        public static TypeReference mini_replace_type(this TypeReference type)
        {
            type = type.Resolve().mono_type_get_underlying_type();
            return mini_native_type_replace_type(type);
        }

        public static TypeReference mini_native_type_replace_type(this TypeReference type)
        {
            return type;
        }


        public static bool mono_type_is_long(this TypeReference type)
        {
            return type.Resolve().mono_type_is_long();
        }

        public static bool mono_type_is_float(this TypeReference type)
        {
            return (!type.IsByReference && ((type.FullName == "System.Single") || (type.FullName == "System.Double")));
        }

        public static bool mono_type_is_long(this TypeDefinition type)
        {
            var underlyingType = type.mono_type_get_underlying_type();
            return !type.IsByReference && ((underlyingType.MetadataType == MetadataType.Int64) || (underlyingType.MetadataType == MetadataType.UInt64));
        }

        public static TypeDefinition mono_class_from_mono_type(this TypeReference type)
        {
            switch (type.MetadataType)
            {
                case MetadataType.Object:
                    return mono_defaults.object_class;
                case MetadataType.Void:
                    return mono_defaults.void_class;
                case MetadataType.Boolean:
                    return mono_defaults.boolean_class;
                case MetadataType.Char:
                    return mono_defaults.char_class;
                case MetadataType.SByte:
                    return mono_defaults.sbyte_class;
                case MetadataType.Byte:
                    return mono_defaults.byte_class;
                case MetadataType.Int16:
                    return mono_defaults.int16_class;
                case MetadataType.UInt16:
                    return mono_defaults.uint16_class;
                case MetadataType.Int32:
                    return mono_defaults.int32_class;
                case MetadataType.UInt32:
                    return mono_defaults.uint32_class;
                case MetadataType.IntPtr:
                    return mono_defaults.int_class;
                case MetadataType.UIntPtr:
                    return mono_defaults.uint_class;
                case MetadataType.Int64:
                    return mono_defaults.int64_class;
                case MetadataType.UInt64:
                    return mono_defaults.uint64_class;
                case MetadataType.Single:
                    return mono_defaults.single_class;
                case MetadataType.Double:
                    return mono_defaults.double_class;
                case MetadataType.String:
                    return mono_defaults.string_class;
                case MetadataType.TypedByReference:
                    return mono_defaults.typed_reference_class;

                //TODO: case MetadataType.Array:
                //    return mono_bounded_array_class_get(type.data.array.eklass, type.data.array.rank, TRUE);
                //TODO: case MetadataType.Pointer:
                //    return mono_ptr_class_get(type.data.type);
                //TODO: case MetadataType.FunctionPointer:
                //    return mono_fnptr_class_get(type.data.method);
                //TODO: case (MetadataType)0x1d: //MONO_TYPE_SZARRAY:
                //    return mono_array_class_get(type.data.klass, 1);
                case MetadataType.Class:
                case MetadataType.ValueType:
                    return type.Resolve();
                //case MetadataType.GenericInstance:
                //    return mono_generic_class_get_class(type.data.generic_class);
                //case MetadataType.Var:
                //    return mono_class_from_generic_parameter(type.data.generic_param, NULL, FALSE);
                //case MetadataType.MVar:
                //    return mono_class_from_generic_parameter(type.data.generic_param, NULL, TRUE);
                default:
                    Helper.Throw(new Exception(string.Format("mono_class_from_mono_type: implement me {0}\n", type.MetadataType)));
                    break;
            }

            return null;
        }

        public static bool mono_type_generic_inst_is_valuetype(this TypeReference retType)
        {
            throw new NotImplementedException();
        }
    }

    static class Mixins
    {
        public static IRegister alloc_dreg(this IMethodContext context, StackType stackType)
        {
            switch (stackType)
            {
                case StackType.STACK_I4:
                case StackType.STACK_PTR:
                    return alloc_ireg(context);
                case StackType.STACK_MP:
                    return alloc_ireg_mp(context);
                case StackType.STACK_OBJ:
                    return alloc_ireg_ref(context);
                case StackType.STACK_R8:
                    return alloc_freg(context);
                case StackType.STACK_I8:
                    return alloc_lreg(context);
                case StackType.STACK_VTYPE:
                    return alloc_ireg(context);
                default:
                    //TODO: g_warning("Unknown stack type %x\n", stackType);
                    Helper.Break();
                    return null;
            }

        }

        public static IRegister alloc_lreg(this IMethodContext context)
        {
            return new Register(context.next_vreg++);
        }

        public static IRegister alloc_freg(this IMethodContext context)
        {
            /* Allocate these from the same pool as the int regs */
            return new Register(context.next_vreg++);
        }

        public static IRegister alloc_ireg_ref(this IMethodContext context)
        {
            var vreg = alloc_ireg(context);

            /* TODO: investigate this in mini code
            if (context.compute_gc_maps)
                mono_mark_vreg_as_ref(cfg, vreg);
            */
            return vreg;
        }

        public static IRegister alloc_ireg_mp(this IMethodContext context)
        {
            var vreg = alloc_ireg(context);

            /* TODO: investigate this in mini code
            if (cfg.compute_gc_maps)
                mono_mark_vreg_as_mp(cfg, vreg);
            */
            return vreg;
        }

        public static IRegister alloc_ireg(this IMethodContext context)
        {
            return new Register(context.next_vreg++);
        }

        public static IRegister alloc_preg(this IMethodContext context)
        {
            return alloc_ireg(context);
        }

        public static TypeReference GetVariableType(this IInstruction inst)
        {
            return (TypeReference)inst.Operand1;
        }

        public static bool IsExceptionType(this TypeDefinition type)
        {
            throw new NotImplementedException();
        }
    }
}