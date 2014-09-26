using System;
using System.Collections.Generic;
using System.Linq;
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
            //if (((sp - stack_start) + n) > header.max_stack) UNVERIFIED();
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

        private InstructionCode mono_type_to_regmove(IMethodContext cfg, TypeReference type)
        {
            if (type.IsByReference)
                return InstructionCode.OP_MOVE;

            type = mini_replace_type(type);

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
                    // if (type.data.klass.enumtype)
                    //{
                    //    type = mono_class_enum_basetype(type.data.klass);
                    //    goto handle_enum;
                    //}
                    //if (MONO_CLASS_IS_SIMD(cfg, mono_class_from_mono_type(type)))
                    //    return InstructionCode.OP_XMOVE;
                    //return InstructionCode.OP_VMOVE;
                case MetadataType.TypedByReference:
                    return InstructionCode.OP_VMOVE;
                case MetadataType.GenericInstance:
                    //TODO: type = type.data.generic_class.container_class.byval_arg;
                    throw new NotImplementedException();
                    goto handle_enum;
                case MetadataType.Var:
                case MetadataType.MVar:
                    throw new NotImplementedException();
                    ////g_assert (cfg.generic_sharing_context);
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


                    //case (MetadataType)0x1d: // MONO_TYPE_SZARRAY
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
                    //if (type.data.klass.enumtype)
                    //{
                    //    type = mono_class_enum_basetype(type.data.klass);
                    //    goto handle_enum;
                    //}
                    //else
                    //{
                    //    inst.klass = klass;
                    //    inst.type = STACK_VTYPE;
                    //    return;
                    //}
                case MetadataType.TypedByReference:
                    inst.klass = mono_defaults.typed_reference_class;
                    inst.StackType = StackType.STACK_VTYPE;
                    return;
                case MetadataType.GenericInstance:
                    //type = &type.data.generic_class.container_class.byval_arg;
                    //goto handle_enum;
                    throw new NotImplementedException();

                case MetadataType.MVar:
                case MetadataType.Var:
                    throw new NotImplementedException();
                    //g_assert(cfg.generic_sharing_context);
                    //if (mini_is_gsharedvt_type(cfg, type))
                    //{
                    //    g_assert(cfg.gsharedvt);
                    //    inst.type = STACK_VTYPE;
                    //}
                    //else
                    //{
                    //    inst.type = STACK_OBJ;
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
                        //if (!dont_verify_stloc && target_type_is_incompatible(cfg, header.locals[n], *sp))
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
                        //    NEW_SEQ_POINT (cfg, ins, ip - header.code, TRUE);
                        //    MONO_ADD_INS (cfg.cbb, ins);
                        //}

                        //g_assert (!return_var);
                        CHECK_STACK(context, 1);
                        //--sp;

                        //if ((method.wrapper_type == MONO_WRAPPER_DYNAMIC_METHOD || method.wrapper_type == MONO_WRAPPER_NONE) && target_type_is_incompatible (cfg, ret_type, *sp))
                        //    UNVERIFIED;

                        if (mini_type_to_stind(context, ret_type) == InstructionCode.CilStobj)
                        {
                            if (context.vret_addr == null)
                            {
                                var ins = EMIT_NEW_VARSTORE(context, context.ReturnType, ret_type, instruction);
                                return instruction.ReplaceWith(ins);
                            }
                            else
                            {
                                var ret_addr = EMIT_NEW_RETLOADA(context);

                                var ins = EMIT_NEW_STORE_MEMBASE(context, InstructionCode.OP_STOREV_MEMBASE,
                                    ret_addr.Destination, 0, instruction.Destination);

                                ins.klass = mono_class_from_mono_type(ret_type);

                                return instruction.ReplaceWith(ins);
                            }
                        }
                        else
                        {
                            mono_arch_emit_setret(context, context.Method, instruction);
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

        private void mono_arch_emit_setret(IMethodContext context, MethodDefinition method, IInstruction instruction)
        {
            throw new NotImplementedException();
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

            var cinfo = get_call_info(cfg, cfg.Method);
            var sig_ret = mini_replace_type(cfg.Method.ReturnType);
            
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

        private TypeReference mini_replace_type(TypeReference returnType)
        {
            throw new NotImplementedException();
        }

        private TypeReference mini_type_get_underlying_type(object gsctx, TypeReference type)
        {
            type = mini_native_type_replace_type(type);

            if (type.IsByReference)
                return mono_defaults.int_class;

            if (!type.IsByReference && (type.MetadataType == MetadataType.Var|| type.MetadataType == MetadataType.MVar) &&
                mini_is_gsharedvt_type_gsctx(gsctx, type))
                return type;
            
            return mini_get_basic_type_from_generic(gsctx, mono_type_get_underlying_type(type.Resolve()));
        }

        private TypeReference mini_get_basic_type_from_generic(object gsctx, TypeReference type)
        {
            if (!type.IsByReference && (type.MetadataType == MetadataType.Var || type.MetadataType == MetadataType.MVar) && mini_is_gsharedvt_type_gsctx(gsctx, type))
                return type;
            
            return mini_native_type_replace_type(mono_type_get_basic_type_from_generic(type));
        }

        private TypeReference mono_type_get_basic_type_from_generic(TypeReference type)
        {
            /* When we do generic sharing we let type variables stand for reference types. */
            if (!type.IsByReference && (type.MetadataType == MetadataType.Var || type.MetadataType == MetadataType.MVar))
                return mono_defaults.object_class;

            return type;
        }

        private TypeReference mini_native_type_replace_type(TypeReference type)
        {
            return type;
        }


        private CallInfo get_call_info(object gsctx, MethodDefinition method)
        {

            int n = (method.HasThis ? 1 : 0) + method.Parameters.Count;
            uint stack_size = 0;
            var isPinvoke = method.IsPInvokeImpl;

            var sig = method;

            var gr = 0U;
            var fr = 0U;

            var cinfo = new CallInfo
            {
                nargs = n, 
                args = new ArgInfo[n], 
                ret = new ArgInfo()
            };

            var param_regs = callconv_param_regs(sig);

            {
                var ret_type = mini_type_get_underlying_type(gsctx, sig.ReturnType);
                switch (ret_type.MetadataType)
                {
                    case MetadataType.Boolean:
                    case MetadataType.SByte:
                    case MetadataType.Byte:
                    case MetadataType.Int16:
                    case MetadataType.UInt16:
                    case MetadataType.Char:
                    case MetadataType.Int32:
                    case MetadataType.UInt32:
                    case MetadataType.IntPtr:
                    case MetadataType.UIntPtr:
                    case MetadataType.Pointer:
                    case MetadataType.FunctionPointer:
                    case MetadataType.Class:
                    case MetadataType.Object:

                    case (MetadataType)0x1d: //MONO_TYPE_SZARRAY:
                    case MetadataType.Array:
                    case MetadataType.String:
                        cinfo.ret.storage = ArgStorage.ArgInIReg;
                        cinfo.ret.reg = X86_Reg_No.X86_EAX;
                        break;
                    case MetadataType.UInt64:
                    case MetadataType.Int64:
                        cinfo.ret.storage = ArgStorage.ArgInIReg;
                        cinfo.ret.reg = X86_Reg_No.X86_EAX;
                        cinfo.ret.is_pair = true;
                        break;
                    case MetadataType.Single:
                        cinfo.ret.storage = ArgStorage.ArgOnFloatFpStack;
                        break;
                    case MetadataType.Double:
                        cinfo.ret.storage = ArgStorage.ArgOnDoubleFpStack;
                        break;
                    case MetadataType.GenericInstance:
                        if (!mono_type_generic_inst_is_valuetype(ret_type))
                        {
                            cinfo.ret.storage = ArgStorage.ArgInIReg;
                            cinfo.ret.reg = X86_Reg_No.X86_EAX;
                            break;
                        }
                        if (mini_is_gsharedvt_type_gsctx(gsctx, ret_type))
                        {
                            cinfo.ret.storage = ArgStorage.ArgOnStack;
                            cinfo.vtype_retaddr = true;
                            break;
                        }

                        goto case MetadataType.ValueType;
                    /* Fall through */
                    case MetadataType.ValueType:
                    case MetadataType.TypedByReference:
                        {
                            uint tmpGr = 0;
                            uint tmpFr = 0, tmpStacksize = 0;
                            var tmpParamRegs = new X86_Reg_No[0];

                            add_valuetype(gsctx, sig, cinfo.ret, ret_type, true, ref tmpGr, tmpParamRegs, ref tmpFr, ref tmpStacksize);
                            if (cinfo.ret.storage == ArgStorage.ArgOnStack)
                            {
                                cinfo.vtype_retaddr = true;
                                /* The caller passes the address where the value is stored */
                            }
                            break;
                        }
                    case MetadataType.Var:
                    case MetadataType.MVar:
                        Helper.True(mini_is_gsharedvt_type_gsctx(gsctx, ret_type));
                        cinfo.ret.storage = ArgStorage.ArgOnStack;
                        cinfo.vtype_retaddr = true;
                        break;
                    case MetadataType.Void:
                        cinfo.ret.storage = ArgStorage.ArgNone;
                        break;
                    default:
                        Helper.Stop("Can't handle as return value {0}", ret_type.MetadataType);
                        break;
                }
            }

            var pstart = 0;
            /*
             * To simplify get_this_arg_reg () and LLVM integration, emit the vret arg after
             * the first argument, allowing 'this' to be always passed in the first arg reg.
             * Also do this if the first argument is a reference type, since virtual calls
             * are sometimes made using calli without sig.hasthis set, like in the delegate
             * invoke wrappers.
             */
            if (cinfo.vtype_retaddr && !isPinvoke &&
                (sig.HasThis ||
                 (sig.Parameters.Count > 0 && MONO_TYPE_IS_REFERENCE(mini_type_get_underlying_type(gsctx, sig.Parameters[0].ParameterType))))
            )
            {
                if (sig.HasThis)
                {
                    add_general(ref gr, ref param_regs, ref stack_size, cinfo.args[0]);
                }
                else
                {
                    add_general(ref gr, ref param_regs, ref stack_size, cinfo.args[0]);
                    pstart = 1;
                }
                cinfo.vret_arg_offset = stack_size;
                X86_Reg_No[] temp = null;
                add_general(ref gr, ref temp, ref stack_size, cinfo.ret);
                cinfo.vret_arg_index = 1;
            }
        else
            {
                /* this */
                if (sig.HasThis)
                    add_general(ref gr, ref param_regs, ref stack_size, cinfo.args[0]);

                if (cinfo.vtype_retaddr)
                {   
                    X86_Reg_No[] temp = null;
                    add_general(ref gr, ref temp, ref stack_size, cinfo.ret);
                }
            }

            if (!sig.IsPInvokeImpl && (sig.CallingConvention == MethodCallingConvention.VarArg) && (n == 0))
            {
                fr = 0; //FLOAT_PARAM_REGS;

                /* Emit the signature cookie just before the implicit arguments */
                add_general(ref gr, ref param_regs, ref stack_size, cinfo.sig_cookie);
            }

            var hasthis = sig.HasThis ? 1 : 0;

            for (var i = pstart; i < sig.Parameters.Count; ++i)
            {
                var ainfo = cinfo.args[hasthis + i];

                if (!sig.IsPInvokeImpl && (sig.CallingConvention == MethodCallingConvention.VarArg) && (false /* i == sig.sentinelpos */))
                {
                    /* We allways pass the sig cookie on the stack for simplicity */
                    /* 
                     * Prevent implicit arguments + the sig cookie from being passed 
                     * in registers.
                     */
                    fr = 0; //FLOAT_PARAM_REGS;

                    /* Emit the signature cookie just before the implicit arguments */
                    add_general(ref gr, ref param_regs, ref stack_size, cinfo.sig_cookie);
                }

                //TODO: not sure exactly what this means: if (sig.Parameters [i].byref)
                if (sig.Parameters[i].ParameterType.IsByReference)
                {
                    add_general(ref gr, ref param_regs, ref stack_size, ainfo);
                    continue;
                }
                var ptype = mini_type_get_underlying_type(gsctx, sig.Parameters[i].ParameterType)
                ;
                switch (ptype.MetadataType)
                {
                    case MetadataType.Boolean:
                    case MetadataType.SByte:
                    case MetadataType.Byte:
                        add_general(ref gr, ref param_regs, ref stack_size, ainfo);
                        break;
                    case MetadataType.Int16:
                    case MetadataType.UInt16:
                    case MetadataType.Char:
                        add_general(ref gr, ref param_regs, ref stack_size, ainfo);
                        break;
                    case MetadataType.Int32:
                    case MetadataType.UInt32:
                        add_general(ref gr, ref param_regs, ref stack_size, ainfo);
                        break;
                    case MetadataType.IntPtr:
                    case MetadataType.UIntPtr:
                    case MetadataType.Pointer:
                    case MetadataType.FunctionPointer:
                    case MetadataType.Class:
                    case MetadataType.Object:
                    case MetadataType.String:
                    case (MetadataType)0x1d: //MONO_TYPE_SZARRAY:
                    case MetadataType.Array:
                        add_general(ref gr, ref param_regs, ref stack_size, ainfo);
                        break;
                    case MetadataType.GenericInstance:
                        if (!mono_type_generic_inst_is_valuetype(ptype))
                        {
                            add_general(ref gr, ref param_regs, ref stack_size, ainfo);
                            break;
                        }
                        if (mini_is_gsharedvt_type_gsctx(gsctx, ptype))
                        {
                            /* gsharedvt arguments are passed by ref */
                            add_general(ref gr, ref param_regs, ref stack_size, ainfo);
                            Helper.True(ainfo.storage == ArgStorage.ArgOnStack);
                            ainfo.storage = ArgStorage.ArgGSharedVt;
                            break;
                        }
                        /* Fall through */
                        goto case MetadataType.ValueType;
                    case MetadataType.ValueType:
                    case MetadataType.TypedByReference:
                        add_valuetype(gsctx, sig, ainfo, ptype, false, ref gr, param_regs, ref fr, ref stack_size);
                        break;
                    case MetadataType.UInt64:
                    case MetadataType.Int64:
                        add_general_pair(ref gr, ref param_regs, ref stack_size, ainfo);
                        break;
                    case MetadataType.Single:
                        add_float(ref fr, ref stack_size, ainfo, false);
                        break;
                    case MetadataType.Double:
                        add_float(ref fr, ref stack_size, ainfo, true);
                        break;
                    case MetadataType.Var:
                    case MetadataType.MVar:
                        /* gsharedvt arguments are passed by ref */
                        Helper.True(mini_is_gsharedvt_type_gsctx(gsctx, ptype));
                        add_general(ref gr, ref param_regs, ref stack_size, ainfo);
                        Helper.True(ainfo.storage == ArgStorage.ArgOnStack);
                        ainfo.storage = ArgStorage.ArgGSharedVt;
                        break;
                    default:
                        Helper.Stop("unexpected type {0}", ptype.MetadataType);
                        break;
                }
            }

        //    if (!sig.pinvoke && (sig.call_convention == MONO_CALL_VARARG) && (n > 0) &&
        //        (sig.sentinelpos == sig.param_count))
        //    {
        //        fr = FLOAT_PARAM_REGS;

        //        /* Emit the signature cookie just before the implicit arguments */
        //        add_general(&gr, param_regs, &stack_size, &cinfo.sig_cookie);
        //    }

        //    if (mono_do_x86_stack_align && (stack_size%MONO_ARCH_FRAME_ALIGNMENT) != 0)
        //    {
        //        cinfo.need_stack_align = TRUE;
        //        cinfo.stack_align_amount = MONO_ARCH_FRAME_ALIGNMENT - (stack_size%MONO_ARCH_FRAME_ALIGNMENT);
        //        stack_size += cinfo.stack_align_amount;
        //    }

        //    if (cinfo.vtype_retaddr)
        //    {
        //        /* if the function returns a struct on stack, the called method already does a ret $0x4 */
        //        cinfo.callee_stack_pop = 4;
        //    }

        //    cinfo.stack_usage = stack_size;
        //    cinfo.reg_usage = gr;
        //    cinfo.freg_usage = fr;
        //    return cinfo;

            //
            throw new NotImplementedException();
        }

        private void add_float(ref uint fr, ref uint stackSize, ArgInfo ainfo, bool p3)
        {
            throw new NotImplementedException();
        }

        private void add_general_pair(ref uint gr, ref X86_Reg_No[] paramRegs, ref uint stackSize, ArgInfo ainfo)
        {
            throw new NotImplementedException();
        }

        private void add_general(ref uint gr, ref X86_Reg_No[] paramRegs, ref uint stackSize, ArgInfo ainfo)
        {
            ainfo.offset = stackSize;

            if (paramRegs == null || paramRegs[gr] == X86_Reg_No.X86_NREG)
            {
                ainfo.storage = ArgStorage.ArgOnStack;
                ainfo.nslots = 1;
                stackSize += (uint)IntPtr.Size;
            }
            else
            {
                ainfo.storage = ArgStorage.ArgInIReg;
                ainfo.reg = paramRegs[gr];
                gr++;
            }
        }

        private bool MONO_TYPE_IS_REFERENCE(object o)
        {
            throw new NotImplementedException();
        }

        private void add_valuetype(object gsctx, MethodDefinition sig, ArgInfo ainfo, TypeReference type, bool is_return, ref uint gr, X86_Reg_No[] param_regs, ref uint fr, ref uint stack_size)
        {
            throw new NotImplementedException();
        }

        private bool mini_is_gsharedvt_type_gsctx(object gsctx, TypeReference retType)
        {
            throw new NotImplementedException();
        }

        private bool mono_type_generic_inst_is_valuetype(TypeReference retType)
        {
            throw new NotImplementedException();
        }

        static readonly X86_Reg_No[] thiscall_param_regs = { X86_Reg_No.X86_ECX, X86_Reg_No.X86_NREG };

        private X86_Reg_No[] callconv_param_regs(MethodDefinition sig)
        {
            if (sig.IsPInvokeImpl)
                return new X86_Reg_No[0];

            switch (sig.CallingConvention)
            {
                case MethodCallingConvention.ThisCall:
                    return thiscall_param_regs;
                default:
                    return new X86_Reg_No[0];
            }
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
            if (type.IsValueType && type.IsEnum && !type.IsByReference)
                return GetEnumUnderlyingType(type);
            //TODO:  investigate further if (type.IsGenericInstance && type.data.generic_class.container_class.enumtype && !type.IsByReference)
            //    return GetEnumUnderlyingType(type.data.generic_class.container_class);
            return type;
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

    internal class CallInfo
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

    internal class ArgInfo
    {
        public uint offset;
        public X86_Reg_No reg;
        public ArgStorage storage;
        public int nslots;
        public bool is_pair;

        /* Only if storage == ArgValuetypeInReg */
        public ArgStorage[] pair_storage = new ArgStorage[2];
        public byte[] pair_regs  = new byte[2];
    }

    internal enum ArgStorage
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

    internal enum X86_Reg_No
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