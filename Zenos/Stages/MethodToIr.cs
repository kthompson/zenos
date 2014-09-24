using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class MethodToIr : InstructionCompilerStage
    {
        public override void Compile(IMethodContext context)
        {
            CreateVars(context);

            //BuildBasicBlocks(context);

            base.Compile(context);
        }

        //private void BuildBasicBlocks(IMethodContext context)
        //{
        //    context.CurrentBasicBlock
        //}

        private void CHECK_STACK_OVF(IMethodContext context, int n)
        {
            //if (((sp - stack_start) + n) > header->max_stack) UNVERIFIED();
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

        private IInstruction EMIT_NEW_LOAD_ARG(IMethodContext context, int num)
        {
            var dest = NEW_ARGLOAD(context, num);
            MONO_ADD_INS(context.CurrentBasicBlock, dest);
            return dest;
        }

        private IInstruction EMIT_NEW_RETLOADA(IMethodContext context)
        {

            var dest = NEW_RETLOADA (context); 
            MONO_ADD_INS(context.CurrentBasicBlock, dest);
            return dest;
        }

        private IInstruction NEW_RETLOADA(IMethodContext context)
        {
            var dest = MONO_INST_NEW(context, InstructionCode.OP_MOVE);
            dest.StackType = StackType.STACK_MP;
            dest.klass = context.ReturnType.klass;

            dest.Source1 = context.vret_addr.Destination;
            dest.Destination = context.alloc_dreg(dest.StackType);
            return dest;
        }

        private IInstruction NEW_ARGLOAD(IMethodContext context, int num)
        {
            return NEW_VARLOAD(context, context.Parameters[num], context.Parameters[num].GetVariableType());
        }

        private void MONO_ADD_INS(BasicBlock b, IInstruction inst)
        {
            if (b.last_ins != null)
            {
                b.last_ins = b.last_ins.SetNext(inst);
            }
            else
            {
                b.code = b.last_ins = inst;
            }
        }

        private IInstruction NEW_VARLOAD(IMethodContext context, IInstruction var, TypeReference vartype)
        {
            var dest = MONO_INST_NEW(context, InstructionCode.OP_MOVE);
            dest.Code = mono_type_to_regmove(context, vartype);
            type_to_eval_stack_type(context, vartype, dest);
            dest.klass = var.klass;
            dest.Source1 = var.Destination;
            dest.Destination = context.alloc_dreg(dest.StackType);
            if (dest.Code == InstructionCode.OP_VMOVE)
                dest.klass = mono_class_from_mono_type(vartype);

            return dest;
        }

        class mono_defaults
        {

            static mono_defaults()
            {
                mono_defaults.corlib = ModuleDefinition.ReadModule(typeof (object).Assembly.Location);

                mono_defaults.object_class = mono_class_from_name(mono_defaults.corlib, "System", "Object");
                Helper.IsNotNull(mono_defaults.object_class);

                mono_defaults.void_class = mono_class_from_name(mono_defaults.corlib, "System", "Void");
                Helper.IsNotNull(mono_defaults.void_class);

                mono_defaults.boolean_class = mono_class_from_name(mono_defaults.corlib, "System", "Boolean");
                Helper.IsNotNull(mono_defaults.boolean_class);

                mono_defaults.byte_class = mono_class_from_name(mono_defaults.corlib, "System", "Byte");
                Helper.IsNotNull(mono_defaults.byte_class);

                mono_defaults.sbyte_class = mono_class_from_name(mono_defaults.corlib, "System", "SByte");
                Helper.IsNotNull(mono_defaults.sbyte_class);

                mono_defaults.int16_class = mono_class_from_name(mono_defaults.corlib, "System", "Int16");
                Helper.IsNotNull(mono_defaults.int16_class);

                mono_defaults.uint16_class = mono_class_from_name(mono_defaults.corlib, "System", "UInt16");
                Helper.IsNotNull(mono_defaults.uint16_class);

                mono_defaults.int32_class = mono_class_from_name(mono_defaults.corlib, "System", "Int32");
                Helper.IsNotNull(mono_defaults.int32_class);

                mono_defaults.uint32_class = mono_class_from_name(mono_defaults.corlib, "System", "UInt32");
                Helper.IsNotNull(mono_defaults.uint32_class);

                mono_defaults.uint_class = mono_class_from_name(mono_defaults.corlib, "System", "UIntPtr");
                Helper.IsNotNull(mono_defaults.uint_class);

                mono_defaults.int_class = mono_class_from_name(mono_defaults.corlib, "System", "IntPtr");
                Helper.IsNotNull(mono_defaults.int_class);

                mono_defaults.int64_class = mono_class_from_name(mono_defaults.corlib, "System", "Int64");
                Helper.IsNotNull(mono_defaults.int64_class);

                mono_defaults.uint64_class = mono_class_from_name(mono_defaults.corlib, "System", "UInt64");
                Helper.IsNotNull(mono_defaults.uint64_class);

                mono_defaults.single_class = mono_class_from_name(mono_defaults.corlib, "System", "Single");
                Helper.IsNotNull(mono_defaults.single_class);

                mono_defaults.double_class = mono_class_from_name(mono_defaults.corlib, "System", "Double");
                Helper.IsNotNull(mono_defaults.double_class);

                mono_defaults.char_class = mono_class_from_name(mono_defaults.corlib, "System", "Char");
                Helper.IsNotNull(mono_defaults.char_class);

                mono_defaults.string_class = mono_class_from_name(mono_defaults.corlib, "System", "String");
                Helper.IsNotNull(mono_defaults.string_class);

                mono_defaults.enum_class = mono_class_from_name(mono_defaults.corlib, "System", "Enum");
                Helper.IsNotNull(mono_defaults.enum_class);

                mono_defaults.array_class = mono_class_from_name(mono_defaults.corlib, "System", "Array");
                Helper.IsNotNull(mono_defaults.array_class);

                mono_defaults.delegate_class = mono_class_from_name(mono_defaults.corlib, "System", "Delegate");
                Helper.IsNotNull(mono_defaults.delegate_class);

                mono_defaults.multicastdelegate_class = mono_class_from_name(mono_defaults.corlib, "System", "MulticastDelegate");
                Helper.IsNotNull(mono_defaults.multicastdelegate_class);


                mono_defaults.typed_reference_class = mono_class_from_name(mono_defaults.corlib, "System", "TypedReference");
                Helper.IsNotNull(mono_defaults.typed_reference_class);
            }

            
            public static ModuleDefinition corlib { get; private set; }

            public static TypeDefinition object_class { get; private set; }
            public static TypeDefinition void_class { get; private set; }
            public static TypeDefinition boolean_class { get; private set; }
            public static TypeDefinition byte_class { get; private set; }
            public static TypeDefinition sbyte_class { get; private set; }
            public static TypeDefinition int16_class { get; private set; }
            public static TypeDefinition uint16_class { get; private set; }
            public static TypeDefinition int32_class { get; private set; }
            public static TypeDefinition uint32_class { get; private set; }
            public static TypeDefinition uint_class { get; private set; }
            public static TypeDefinition int_class { get; private set; }
            public static TypeDefinition int64_class { get; private set; }
            public static TypeDefinition uint64_class { get; private set; }
            public static TypeDefinition single_class { get; private set; }
            public static TypeDefinition double_class { get; private set; }
            public static TypeDefinition char_class { get; private set; }
            public static TypeDefinition string_class { get; private set; }
            public static TypeDefinition enum_class { get; private set; }
            public static TypeDefinition array_class { get; private set; }
            public static TypeDefinition delegate_class { get; private set; }
            public static TypeDefinition multicastdelegate_class { get; private set; }
            public static TypeDefinition typed_reference_class { get; private set; }
            

            private static TypeDefinition mono_class_from_name(ModuleDefinition corlib, string namespaceName, string typeName)
            {
                return corlib.GetType(namespaceName, typeName);
            }
        }

        private TypeDefinition mono_class_from_mono_type(TypeReference type)
        {
            switch (type.MetadataType)
            {
                case MetadataType.Object:
                    return  mono_defaults.object_class;
                case MetadataType.Void:
                    return  mono_defaults.void_class;
                case MetadataType.Boolean:
                    return  mono_defaults.boolean_class;
                case MetadataType.Char:
                    return  mono_defaults.char_class;
                case MetadataType.SByte:
                    return  mono_defaults.sbyte_class;
                case MetadataType.Byte:
                    return  mono_defaults.byte_class;
                case MetadataType.Int16:
                    return  mono_defaults.int16_class;
                case MetadataType.UInt16:
                    return  mono_defaults.uint16_class;
                case MetadataType.Int32:
                    return  mono_defaults.int32_class;
                case MetadataType.UInt32:
                    return  mono_defaults.uint32_class;
                case MetadataType.IntPtr:
                    return  mono_defaults.int_class;
                case MetadataType.UIntPtr:
                    return  mono_defaults.uint_class;
                case MetadataType.Int64:
                    return  mono_defaults.int64_class;
                case MetadataType.UInt64:
                    return  mono_defaults.uint64_class;
                case MetadataType.Single:
                    return  mono_defaults.single_class;
                case MetadataType.Double:
                    return  mono_defaults.double_class;
                case MetadataType.String:
                    return mono_defaults.string_class;
                case MetadataType.TypedByReference:
                    return mono_defaults.typed_reference_class;

                //TODO: case MetadataType.Array:
                //    return mono_bounded_array_class_get(type->data.array->eklass, type->data.array->rank, TRUE);
                //TODO: case MetadataType.Pointer:
                //    return mono_ptr_class_get(type->data.type);
                //TODO: case MetadataType.FunctionPointer:
                //    return mono_fnptr_class_get(type->data.method);
                //case MONO_TYPE_SZARRAY:
                //    return mono_array_class_get(type->data.klass, 1);
                case MetadataType.Class:
                case MetadataType.ValueType:
                    return type.Resolve();
                //case MetadataType.GenericInstance:
                //    return mono_generic_class_get_class(type->data.generic_class);
                //case MetadataType.Var:
                //    return mono_class_from_generic_parameter(type->data.generic_param, NULL, FALSE);
                //case MetadataType.MVar:
                //    return mono_class_from_generic_parameter(type->data.generic_param, NULL, TRUE);
                default:
                    Helper.Stop(() => new Exception(string.Format("mono_class_from_mono_type: implement me {0}\n", type.MetadataType)));
                    break;
            }

            return null;
        }

        private InstructionCode mono_type_to_regmove(IMethodContext cfg, TypeReference type)
        {
            if (type.IsByReference)
                return InstructionCode.OP_MOVE;

            handle_enum:
            switch (type.MetadataType)
            {
                case MetadataType.SByte:
                case MetadataType.Byte:
                case MetadataType.Boolean:
                    return InstructionCode.OP_MOVE;
                case MetadataType.Int16:
                case MetadataType.UInt16:
                case MetadataType.Char:
                    return InstructionCode.OP_MOVE;
                case MetadataType.Int32:
                case MetadataType.UInt32:
                    return InstructionCode.OP_MOVE;
                case MetadataType.IntPtr:
                case MetadataType.UIntPtr:
                case MetadataType.Pointer:
                case MetadataType.FunctionPointer:
                    return InstructionCode.OP_MOVE;
                case MetadataType.Class:
                case MetadataType.String:
                case MetadataType.Object:
                case MetadataType.Array:
                case (MetadataType) 0x1d: /* ElementType.SzArray */
                    return InstructionCode.OP_MOVE;
                case MetadataType.Int64:
                case MetadataType.UInt64:
                    return InstructionCode.OP_LMOVE;
                case MetadataType.Single:
                    return InstructionCode.OP_FMOVE;
                case MetadataType.Double:
                    return InstructionCode.OP_FMOVE;
                case MetadataType.ValueType:
                    throw new NotImplementedException();
                    //TODO:
                    // if (type->data.klass->enumtype)
                    //{
                    //    type = mono_class_enum_basetype(type->data.klass);
                    //    goto handle_enum;
                    //}
                    //if (MONO_CLASS_IS_SIMD(cfg, mono_class_from_mono_type(type)))
                    //    return InstructionCode.OP_XMOVE;
                    //return InstructionCode.OP_VMOVE;
                case MetadataType.TypedByReference:
                    return InstructionCode.OP_VMOVE;
                case MetadataType.GenericInstance:
                    //TODO: type = type->data.generic_class->container_class->byval_arg;
                    throw new NotImplementedException();
                    goto handle_enum;
                case MetadataType.Var:
                case MetadataType.MVar:
                    throw new NotImplementedException();
                    ////g_assert (cfg->generic_sharing_context);
                    //if (mini_type_var_is_vt(cfg, type))
                    //    return InstructionCode.OP_VMOVE;
                    //else
                    //    return InstructionCode.OP_MOVE;
                default:
                    Helper.Stop(string.Format("unknown type 0x{0} in type_to_regstore", type.MetadataType));
                    break;
            }
            return InstructionCode.Unknown;
        }

        private void type_to_eval_stack_type(IMethodContext context, TypeReference type, IInstruction inst)
        {
            inst.klass = mono_class_from_mono_type(type);
            if (type.IsByReference)
            {
                inst.StackType = StackType.STACK_MP; 
                return;
            }

        handle_enum:
            switch (type.MetadataType)
            {
                case MetadataType.Void:

                    inst.StackType = StackType.STACK_INV;
                    return;
                case MetadataType.Byte:
                case MetadataType.SByte:
                case MetadataType.Boolean:
                case MetadataType.Int16:
                case MetadataType.UInt16:
                case MetadataType.Char:
                case MetadataType.Int32:
                case MetadataType.UInt32:
                    inst.StackType = StackType.STACK_I4;
                    return;
                case MetadataType.IntPtr:
                case MetadataType.UIntPtr:
                case MetadataType.Pointer:
                case MetadataType.FunctionPointer:

                    inst.StackType = StackType.STACK_PTR;
                    return;
                case MetadataType.Class:
                case MetadataType.String:
                case MetadataType.Object:
                case MetadataType.Array:

                    //case MONO_TYPE_SZARRAY:
                    inst.StackType = StackType.STACK_OBJ;
                    return;
                case MetadataType.Int64:
                case MetadataType.UInt64:
                    inst.StackType = StackType.STACK_I8;
                    return;
                case MetadataType.Single:
                case MetadataType.Double:
                    inst.StackType = StackType.STACK_R8;
                    return;
                case MetadataType.ValueType:

                    throw new NotImplementedException();
                    //if (type->data.klass->enumtype)
                    //{
                    //    type = mono_class_enum_basetype(type->data.klass);
                    //    goto handle_enum;
                    //}
                    //else
                    //{
                    //    inst->klass = klass;
                    //    inst->type = STACK_VTYPE;
                    //    return;
                    //}
                case MetadataType.TypedByReference:
                    inst.klass = mono_defaults.typed_reference_class;
                    inst.StackType = StackType.STACK_VTYPE;
                    return;
                case MetadataType.GenericInstance:
                    //type = &type->data.generic_class->container_class->byval_arg;
                    //goto handle_enum;
                    throw new NotImplementedException();

                case MetadataType.MVar:
                case MetadataType.Var:
                    throw new NotImplementedException();
                    //g_assert(cfg->generic_sharing_context);
                    //if (mini_is_gsharedvt_type(cfg, type))
                    //{
                    //    g_assert(cfg->gsharedvt);
                    //    inst->type = STACK_VTYPE;
                    //}
                    //else
                    //{
                    //    inst->type = STACK_OBJ;
                    //}
                    //return;
                default:
                    Helper.Stop(string.Format("unknown type {0} in eval stack type", type.MetadataType));
                    break;
            }
        }

        private IInstruction MONO_INST_NEW(IMethodContext context, InstructionCode op)
        {
            return new IrInstruction {Code = op};
        }

        private IInstruction EMIT_NEW_ICONST(IMethodContext context, int val)
        {
            var dest = NEW_ICONST(context, val);
            MONO_ADD_INS(context.CurrentBasicBlock, (dest));
            return dest;
        }

        private IInstruction NEW_ICONST(IMethodContext context, int val)
        {
            var dest = MONO_INST_NEW(context, InstructionCode.OP_ICONST);
            dest.Operand0 = val;
            dest.StackType = StackType.STACK_I4;
            dest.Destination = context.alloc_dreg(StackType.STACK_I4);
            return dest;
        }

        private IInstruction EMIT_NEW_LOCLOAD(IMethodContext cfg, int num)
        {
            var dest = NEW_LOCLOAD(cfg, num);
            MONO_ADD_INS(cfg.CurrentBasicBlock, (dest));
            return dest;
        }

        private IInstruction NEW_LOCLOAD(IMethodContext context, int num)
        {
            return NEW_VARLOAD(context, context.Variables[num], context.VariableDefinitions[num].VariableType);
        }

        //mono_method_to_ir
        public override IInstruction Compile(IMethodContext context, IInstruction instruction)
        {
            switch (instruction.Code)
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
                        var ins = EMIT_NEW_ICONST(context, (instruction.Code) - InstructionCode.CilLdc_I4_0);
                        return instruction.ReplaceWith(ins);
                        //TODO:
                        //ip++;
                        //*sp++ = ins;
                        break;
                    }
                case InstructionCode.CilLdarg_0:
                case InstructionCode.CilLdarg_1:
                case InstructionCode.CilLdarg_2:
                case InstructionCode.CilLdarg_3:
                    {
                        CHECK_STACK_OVF(context, 1);
                        var n = (instruction.Code) - InstructionCode.CilLdarg_0;
                        CHECK_ARG(context, n);
                        var ins = EMIT_NEW_LOAD_ARG(context, n);
                        return instruction.ReplaceWith(ins);
                        //TODO:
                        //ip++;
                        //*sp++ = ins;

                        break;
                    }
                case InstructionCode.CilLdloc_0:
                case InstructionCode.CilLdloc_1:
                case InstructionCode.CilLdloc_2:
                case InstructionCode.CilLdloc_3:
                    {
                        CHECK_STACK_OVF(context, 1);
                        var n = (instruction.Code) - InstructionCode.CilLdloc_0;
                        CHECK_LOCAL(context, n);
                        var ins = EMIT_NEW_LOCLOAD(context, n);
                        return instruction.ReplaceWith(ins);
                        //ip++;
                        //*sp++ = ins;
                        break;
                    }

                case InstructionCode.CilStloc_0:
                case InstructionCode.CilStloc_1:
                case InstructionCode.CilStloc_2:
                case InstructionCode.CilStloc_3:
                    {
                        CHECK_STACK(context, 1);
                        var n = (instruction.Code) - InstructionCode.CilStloc_0;
                        CHECK_LOCAL(context, n);

                        /* basically we need to look at the previous instr to see if we can do a small optimiation
                         * once we do it we need to skip the current instr(CilStloc) and continue with the one after
                         */
                        instruction = instruction.Previous;
                        //if (!dont_verify_stloc && target_type_is_incompatible(cfg, header->locals[n], *sp))
                        //    UNVERIFIED;
                        emit_stloc_ir(context, instruction, n);
                        instruction.Next.Remove();
                        //++ip;
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
                    //    NEW_SEQ_POINT (cfg, ins, ip - header->code, TRUE);
                    //    MONO_ADD_INS (cfg->cbb, ins);
                    //}

                    //g_assert (!return_var);
					CHECK_STACK (context, 1);
					//--sp;

                    //if ((method->wrapper_type == MONO_WRAPPER_DYNAMIC_METHOD || method->wrapper_type == MONO_WRAPPER_NONE) && target_type_is_incompatible (cfg, ret_type, *sp))
                    //    UNVERIFIED;

					if (mini_type_to_stind (context, ret_type) == InstructionCode.CilStobj) {
						

                        if (context.vret_addr == null)
                        {
                            var ins = EMIT_NEW_VARSTORE(context, context.ReturnType, ret_type, instruction);
                            return instruction.ReplaceWith(ins);
                        } else {
                            var ret_addr = EMIT_NEW_RETLOADA(context);

                            var ins = EMIT_NEW_STORE_MEMBASE(context, InstructionCode.OP_STOREV_MEMBASE, ret_addr.Destination, 0, instruction.Destination);

							ins.klass = mono_class_from_mono_type (ret_type);

                            return instruction.ReplaceWith(ins);
                        }
					} else {
                        mono_arch_emit_setret(context, method, *sp);
					}
				}
                    
                    break;
                }
                default:
                    Helper.Stop("Unsupported instruction: " + instruction.Code);
                    break;
            }



            return instruction;
        }

        private IInstruction EMIT_NEW_STORE_MEMBASE(IMethodContext cfg, InstructionCode op, IRegister @base, int offset, IRegister sr)
        {
            var dest = NEW_STORE_MEMBASE(cfg, op, @base, offset, sr);

            MONO_ADD_INS(cfg.CurrentBasicBlock, dest);

            return dest;
        }

        private IInstruction NEW_STORE_MEMBASE(IMethodContext cfg, InstructionCode op, IRegister @base, int offset, IRegister sr)
        {
            var inst = MONO_INST_NEW (cfg, op); 
            inst.Source1 = sr; 
            inst.Destination = @base; 
            inst.Operand0 = offset;
            MONO_ADD_INS(cfg.CurrentBasicBlock, inst);
            return inst;
        }

        private InstructionCode mini_type_to_stind(IMethodContext cfg, TypeReference type)
        {
            //if (cfg->generic_sharing_context && !type.IsByReference)
            //{
            //    if (type.type == MONO_TYPE_VAR || type->type == MONO_TYPE_MVAR)
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
                    if (type.data.klass->enumtype)
                    {
                        type = mono_class_enum_basetype(type->data.klass);
                        goto handle_enum;
                    }
                    return InstructionCode.CilStobj;
                case MetadataType.TypedByReference:
                    return InstructionCode.CilStobj;
                case MetadataType.GenericInstance:
                    type = type->data.generic_class->container_class->byval_arg;
                    goto handle_enum;
                default:
                    Helper.Stop("unknown type 0x%02x in type_to_stind", type.MetadataType);
            }
        }

        private void emit_stloc_ir(IMethodContext cfg, IInstruction sp, int n)
        {
            var opcode = mono_type_to_regmove(cfg, cfg.Locals[n].GetVariableType());
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
                var ins = EMIT_NEW_LOCSTORE(cfg, n, sp);
                //TODO: should this have sp.ReplaceWith(ins)?
                Helper.Break();
            }
        }

        private IInstruction EMIT_NEW_LOCSTORE(IMethodContext cfg, int num, IInstruction inst)
        {
            var dest = NEW_LOCSTORE(cfg, num, inst); 
            MONO_ADD_INS(cfg.CurrentBasicBlock, dest);
            return dest;
        }

        private IInstruction EMIT_NEW_VARSTORE(IMethodContext cfg, IInstruction var, TypeReference vartype, IInstruction inst)
        {
            var dest = NEW_VARSTORE (cfg, var, vartype, inst);
            MONO_ADD_INS(cfg.CurrentBasicBlock, dest);
            return dest;
        } 

        private IInstruction NEW_LOCSTORE(IMethodContext cfg, int num, IInstruction inst)
        {
            return NEW_VARSTORE(cfg, cfg.Variables[num], cfg.Variables[num].GetVariableType(), inst);
        }

        private IInstruction NEW_VARSTORE(IMethodContext cfg, IInstruction var, TypeReference vartype, IInstruction inst)
        {
            var dest = MONO_INST_NEW(cfg, InstructionCode.OP_MOVE);
            dest.Code = mono_type_to_regmove(cfg, vartype);
            dest.klass = var.klass;
            dest.Source1 = inst.Destination;
            dest.Destination = var.Destination;
            if (dest.Code == InstructionCode.OP_VMOVE)
                dest.klass = mono_class_from_mono_type((vartype));

            return dest;
        }


        private void CHECK_STACK(IMethodContext context, int num)
        {
            //if ((sp - stack_start) < (num)) UNVERIFIED();
        }


        private void CreateVars(IMethodContext context)
        {
            //mono_compile_create_vars
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

            //if (context.Method.->save_lmf && context->create_lmf_var)
            //{
            //    var lmf_var = mono_compile_create_var(context, !mono_defaults.int_class.IsByReference, InstructionCode.OP_LOCAL);
            //    lmf_var.Flags |= InstructionFlags.Volatile;
            //    lmf_var.Flags |= InstructionFlags.Lmf;
            //    context.lmf_var = lmf_var;
            //}
        }

        private void mono_arch_create_vars(IMethodContext context)
        {
            throw new NotImplementedException();
        }

        private bool mono_type_is_long(TypeReference type)
        {
            return mono_type_is_long(type.Resolve());
        }

        private bool mono_type_is_long(TypeDefinition type)
        {
            return !type.IsByReference && ((mono_type_get_underlying_type(type).MetadataType == MetadataType.Int64) || (mono_type_get_underlying_type(type).MetadataType == MetadataType.UInt64));
        }

        private TypeDefinition mono_type_get_underlying_type(TypeDefinition type)
        {

            if (type.IsValueType && IsEnum(type) && !type.IsByReference)
                return GetEnumUnderlyingType(type);
            //TODO:  investigate further if (type.IsGenericInstance && type->data.generic_class->container_class->enumtype && !type.IsByReference)
            //    return GetEnumUnderlyingType(type->data.generic_class->container_class);
            return type;
        }

        private bool IsEnum(TypeDefinition self)
        {
            return self.BaseType != null && self.BaseType.FullName == "System.Enum";
        }

        public static TypeDefinition GetEnumUnderlyingType(TypeDefinition self)
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

        private bool mono_type_is_float(TypeReference type)
        {
            return (!type.IsByReference && ((type.FullName == "System.Single") || (type.FullName == "System.Double")));
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
            var vi = MONO_INIT_VARINFO(cfg);
            vi.VirtualRegister = vreg;

            var inst = MONO_INST_NEW(cfg, opcode);
            inst.Operand0 = vi;
            inst.Operand1 = type;
            inst.klass = mono_class_from_mono_type(type);
            type_to_eval_stack_type(cfg, type, inst);
            //    /* if set to 1 the variable is native */
            //    inst->backend.is_pinvoke = 0;
            inst.Destination = vreg;

            //if (inst.klass.IsExceptionType())
            //    mono_cfg_set_exception(cfg, MONO_EXCEPTION_TYPE_LOAD);

            //    if (cfg->compute_gc_maps) {
            //        if (type->byref) {
            //            mono_mark_vreg_as_mp (cfg, vreg);
            //        } else {
            //            MonoType *t = mini_type_get_underlying_type (NULL, type);
            //            if ((MONO_TYPE_ISSTRUCT (t) && inst->klass->has_references) || mini_type_is_reference (cfg, t)) {
            //                inst->flags |= MONO_INST_GC_TRACK;
            //                mono_mark_vreg_as_ref (cfg, vreg);
            //            }
            //        }
            //    }

            cfg.Variables.Add(inst);


            if (vreg != null)
                set_vreg_to_inst(cfg, vreg, inst);

            //#ifdef MONO_ARCH_SOFT_FLOAT
            bool regpair = mono_type_is_long(type) || mono_type_is_float(type);
            //#else
            //    regpair = mono_type_is_long (type);
            //#endif


            if (regpair)
            {
                IInstruction tree;

                /* 
                     * These two cannot be allocated using create_var_for_vreg since that would
                     * put it into the cfg->varinfo array, confusing many parts of the JIT.
                     */

                /* 
                     * Set flags to VOLATILE so SSA skips it.
                     */

                //if (cfg.verbose_level >= 4) {
                //    printf ("  Create LVAR R%d (R%d, R%d)\n", inst->dreg, inst->dreg + 1, inst->dreg + 2);
                //}


                //if (cfg.opt & MONO_OPT_SSA) {
                //    if (mono_type_is_float (type))
                //        inst.flags = MONO_INST_VOLATILE;
                //}


                /* Allocate a dummy MonoInst for the first vreg */
                tree = MONO_INST_NEW(cfg, InstructionCode.OP_LOCAL);
                tree.Destination = new Register(inst.Destination.Id + 1);
                //if (cfg.opt & MONO_OPT_SSA)
                //    tree.flags = MONO_INST_VOLATILE;
                tree.Operand0 = vi;
                tree.StackType = StackType.STACK_I4;
                tree.Operand1 = mono_defaults.int32_class;
                tree.klass = mono_class_from_mono_type(mono_defaults.int32_class);

                set_vreg_to_inst(cfg, tree.Destination, tree);

                /* Allocate a dummy MonoInst for the second vreg */
                tree = MONO_INST_NEW(cfg, InstructionCode.OP_LOCAL);
                tree.Destination = new Register(inst.Destination.Id + 2);
                //if (cfg.opt & MONO_OPT_SSA)
                //    tree.flags = MONO_INST_VOLATILE;
                tree.Operand0 = vi;
                tree.StackType = StackType.STACK_I4;
                tree.Operand1 = mono_defaults.int32_class;
                tree.klass = mono_class_from_mono_type(mono_defaults.int32_class);

                set_vreg_to_inst(cfg, tree.Destination, tree);
            }

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

        private IVariableDefinition MONO_INIT_VARINFO(IMethodContext cfg)
        {
            var vi = new VariableDefinition {Index = cfg.VariableDefinitions.Count};
            cfg.VariableDefinitions.Add(vi);
            //vi.range.first_use.pos.bid = 0xffff; 
            return vi;
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
            if (cfg->compute_gc_maps)
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