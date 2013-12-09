using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Zenos.Core;

namespace Zenos.Framework
{
    public class MethodToIr : InstructionCompilerStage
    {
        public override void Compile(IMethodContext context)
        {
            CreateVars(context);

            base.Compile(context);
        }

        private void CHECK_STACK_OVF(IMethodContext context, int n)
        {
            //if (((sp - stack_start) + n) > header->max_stack) UNVERIFIED
        }

        private void CHECK_LOCAL(IMethodContext context, int num)
        {
            if (num >= context.Variables.Length)
                UNVERIFIED();
        }


        private void CHECK_ARG(IMethodContext context, int num)
        {
            if (num >= context.Parameters.Length)
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

        private IInstruction NEW_ARGLOAD(IMethodContext context, int num)
        {
            return NEW_VARLOAD(context, context.Parameters[num], context.ParameterDefinitions[num].ParameterType);
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

        private InstructionCode mono_type_to_regmove(IMethodContext context, TypeReference vartype)
        {
            throw new NotImplementedException();
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
            dest.Operand = val;
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
                default:
                    Helper.Break();
                    break;
            }

            return instruction;
        }



        private void CreateVars(IMethodContext context)
        {
            //mono_compile_create_vars
            var method = context.Method;

            if (method.ReturnType.FullName != "System.Void")
                context.ReturnType = mono_compile_create_var(context, method.ReturnType, InstructionCode.OP_ARG);

            var count = method.Parameters.Count;
            var hasThis = method.HasThis ? 1 : 0;
            context.Parameters = new IInstruction[count + hasThis];

            if (method.HasThis)
                context.Parameters[0] = mono_compile_create_var(context, method.Body.ThisParameter.ParameterType, InstructionCode.OP_ARG);

            for (var i = 0; i < count; ++i)
            {
                context.Parameters[i + hasThis] = mono_compile_create_var(context, method.Parameters[i].ParameterType, InstructionCode.OP_ARG);
            }


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
            //    int num = cfg.num_varinfo;
            //    bool regpair;
            //// NOTE: this code appears to reallocate our variables into new arrays and then copy the existing ones over.
            //    if ((num + 1) >= cfg->varinfo_count) {
            //        int orig_count = cfg->varinfo_count;
            //        cfg->varinfo_count = cfg->varinfo_count ? (cfg->varinfo_count * 2) : 64;
            //        cfg->varinfo = (MonoInst **)g_realloc (cfg->varinfo, sizeof (MonoInst*) * cfg->varinfo_count);
            //        cfg->vars = (MonoMethodVar *)g_realloc (cfg->vars, sizeof (MonoMethodVar) * cfg->varinfo_count);
            //        memset (&cfg->vars [orig_count], 0, (cfg->varinfo_count - orig_count) * sizeof (MonoMethodVar));
            //    }

            //    cfg->stat_allocate_var++;

            var inst = MONO_INST_NEW(cfg, opcode);
            //    inst->inst_c0 = num;
            //    inst->inst_vtype = type;
            inst.klass = mono_class_from_mono_type (type);
            type_to_eval_stack_type (cfg, type, inst);
            //    /* if set to 1 the variable is native */
            //    inst->backend.is_pinvoke = 0;
            //    inst->dreg = vreg;

            //    if (inst->klass->exception_type)
            //        mono_cfg_set_exception (cfg, MONO_EXCEPTION_TYPE_LOAD);

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

            //    cfg->varinfo [num] = inst;

            //    MONO_INIT_VARINFO (&cfg->vars [num], num);
            //    MONO_VARINFO (cfg, num)->vreg = vreg;

            //    if (vreg != -1)
            //        set_vreg_to_inst (cfg, vreg, inst);

            //#if SIZEOF_REGISTER == 4
            //#ifdef MONO_ARCH_SOFT_FLOAT
            //    regpair = mono_type_is_long (type) || mono_type_is_float (type);
            //#else
            //    regpair = mono_type_is_long (type);
            //#endif
            //#else
            //    regpair = FALSE;
            //#endif

            //    if (regpair) {
            //        MonoInst *tree;

            //        /* 
            //         * These two cannot be allocated using create_var_for_vreg since that would
            //         * put it into the cfg->varinfo array, confusing many parts of the JIT.
            //         */

            //        /* 
            //         * Set flags to VOLATILE so SSA skips it.
            //         */

            //        if (cfg->verbose_level >= 4) {
            //            printf ("  Create LVAR R%d (R%d, R%d)\n", inst->dreg, inst->dreg + 1, inst->dreg + 2);
            //        }

            //#ifdef MONO_ARCH_SOFT_FLOAT
            //        if (cfg->opt & MONO_OPT_SSA) {
            //            if (mono_type_is_float (type))
            //                inst->flags = MONO_INST_VOLATILE;
            //        }
            //#endif

            //        /* Allocate a dummy MonoInst for the first vreg */
            //        MONO_INST_NEW (cfg, tree, OP_LOCAL);
            //        tree->dreg = inst->dreg + 1;
            //        if (cfg->opt & MONO_OPT_SSA)
            //            tree->flags = MONO_INST_VOLATILE;
            //        tree->inst_c0 = num;
            //        tree->type = STACK_I4;
            //        tree->inst_vtype = &mono_defaults.int32_class->byval_arg;
            //        tree->klass = mono_class_from_mono_type (tree->inst_vtype);

            //        set_vreg_to_inst (cfg, inst->dreg + 1, tree);

            //        /* Allocate a dummy MonoInst for the second vreg */
            //        MONO_INST_NEW (cfg, tree, OP_LOCAL);
            //        tree->dreg = inst->dreg + 2;
            //        if (cfg->opt & MONO_OPT_SSA)
            //            tree->flags = MONO_INST_VOLATILE;
            //        tree->inst_c0 = num;
            //        tree->type = STACK_I4;
            //        tree->inst_vtype = &mono_defaults.int32_class->byval_arg;
            //        tree->klass = mono_class_from_mono_type (tree->inst_vtype);

            //        set_vreg_to_inst (cfg, inst->dreg + 2, tree);
            //    }

            //    cfg->num_varinfo++;
            //    if (cfg->verbose_level > 2)
            //        g_print ("created temp %d (R%d) of type %s\n", num, vreg, mono_type_get_name (type));
            //    return inst;
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
    }
}