using System;
using System.Security.Cryptography;
using Mono.Cecil;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class Emitter
    {
        public IInstruction EMIT_NEW_LOAD_ARG(IMethodContext context, int num)
        {
            var dest = NEW_ARGLOAD(context, num);
            MONO_ADD_INS(context.CurrentBasicBlock, dest);
            return dest;
        }

        public IInstruction EMIT_NEW_RETLOADA(IMethodContext context)
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

        public void MONO_ADD_INS(BasicBlock b, IInstruction inst)
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
                dest.klass = (vartype).mono_class_from_mono_type();

            return dest;
        }

        public IInstruction MONO_INST_NEW(IMethodContext context, InstructionCode op)
        {
            return new IrInstruction {Code = op};
        }

        public IInstruction EMIT_NEW_ICONST(IMethodContext context, int val)
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

        public IInstruction EMIT_NEW_LOCLOAD(IMethodContext cfg, int num)
        {
            var dest = NEW_LOCLOAD(cfg, num);
            MONO_ADD_INS(cfg.CurrentBasicBlock, (dest));
            return dest;
        }

        private IInstruction NEW_LOCLOAD(IMethodContext context, int num)
        {
            return NEW_VARLOAD(context, context.Variables[num], context.VariableDefinitions[num].VariableType);
        }

        public IInstruction EMIT_NEW_STORE_MEMBASE(IMethodContext cfg, InstructionCode op, IRegister @base, int offset, IRegister sr)
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

        public IInstruction EMIT_NEW_LOCSTORE(IMethodContext cfg, int num, IInstruction inst)
        {
            var dest = NEW_LOCSTORE(cfg, num, inst); 
            MONO_ADD_INS(cfg.CurrentBasicBlock, dest);
            return dest;
        }

        public IInstruction EMIT_NEW_VARSTORE(IMethodContext cfg, IInstruction var, TypeReference vartype, IInstruction inst)
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
                dest.klass = vartype.mono_class_from_mono_type();

            return dest;
        }

        public void MONO_EMIT_NEW_UNALU(IMethodContext cfg, InstructionCode op, IRegister dr, IRegister sr1)
        {
            var inst = EMIT_NEW_UNALU (cfg, op, dr, sr1);
        }

        private IInstruction EMIT_NEW_UNALU(IMethodContext cfg, InstructionCode op, IRegister dr, IRegister sr1)
        {
            var dest = NEW_UNALU((cfg), op, (dr), (sr1)); 
            MONO_ADD_INS(cfg.CurrentBasicBlock, dest);
            return dest;
        }

        private IInstruction NEW_UNALU(IMethodContext cfg, InstructionCode op, IRegister dr, IRegister sr1)
        {
            var dest = MONO_INST_NEW((cfg), (op));
            dest.Destination = dr;
            dest.Source1 = sr1;
            return dest;
        }

        public InstructionCode mono_type_to_regmove(IMethodContext cfg, TypeReference type)
        {
            if (type.IsByReference)
                return InstructionCode.OP_MOVE;

            type = type.mini_replace_type();

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
                case (MetadataType)0x1d: /* ElementType.SzArray */
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

        public void type_to_eval_stack_type(IMethodContext context, TypeReference type, IInstruction inst)
        {
            inst.klass = type.mono_class_from_mono_type();
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

        public IInstruction EMIT_NEW_TEMPLOAD(IMethodContext cfg, int num)
        {
            var dest = NEW_TEMPLOAD(cfg, num); 
            MONO_ADD_INS(cfg.CurrentBasicBlock, dest);

            return dest;
        }

        private IInstruction NEW_TEMPLOAD(IMethodContext cfg, int num)
        {
            return NEW_VARLOAD(cfg, cfg.Variables[num], cfg.Variables[num].inst_vtype());
        }


        public void emit_init_local(IMethodContext context, int local, TypeReference type, bool init)
        {
            var var = context.Variables[local];
            //if (COMPILE_SOFT_FLOAT(cfg))
            //{
            //    MonoInst* store;
            //    int reg = alloc_dreg(cfg, var->type);
            //    emit_init_rvar(cfg, reg, type);
            //    EMIT_NEW_LOCSTORE(cfg, store, local, cfg->cbb->last_ins);
            //}
            //else
            {
                if (init)
                    emit_init_rvar(context, var.Destination, type);
                else
                    emit_dummy_init_rvar(context, var.Destination, type);
            }
        }

        private void emit_dummy_init_rvar(IMethodContext context, IRegister destination, TypeReference type)
        {
            throw new NotImplementedException();
        }

        static double r8_0 = 0.0;

        private void emit_init_rvar(IMethodContext cfg, IRegister dreg, TypeReference rtype)
        {
            rtype = rtype.mini_replace_type();
            var t = rtype.MetadataType;

            if (rtype.IsByReference)
            {
                MONO_EMIT_NEW_PCONST(cfg, dreg, null);
            }
            else if (t >= MetadataType.Boolean && t <= MetadataType.UInt32)
            {
                MONO_EMIT_NEW_ICONST(cfg, dreg, 0);
            }
            else if (t == MetadataType.Int64 || t == MetadataType.UInt64)
            {
                Helper.Stop();
                //MONO_EMIT_NEW_I8CONST(cfg, dreg, 0);
            }
            else if (t == MetadataType.Single || t == MetadataType.Double)
            {
                var ins = MONO_INST_NEW(cfg, InstructionCode.OP_R8CONST);
                ins.StackType = StackType.STACK_R8;
                Helper.Stop("ins->inst_p0 = (void*) &r8_0;");
                ins.Destination = dreg;
                MONO_ADD_INS(cfg.CurrentBasicBlock, ins);
            }
            else if ((t == MetadataType.ValueType) || (t == MetadataType.TypedByReference) ||
                     ((t == MetadataType.GenericInstance) && rtype.mono_type_generic_inst_is_valuetype()))
            {
                Helper.Stop();
                //MONO_EMIT_NEW_VZERO(cfg, dreg, mono_class_from_mono_type(rtype));
            }
            else if (((t == MetadataType.Var) || (t == MetadataType.MVar)) && mini_type_var_is_vt(cfg, rtype))
            {
                Helper.Stop();
                //MONO_EMIT_NEW_VZERO(cfg, dreg, mono_class_from_mono_type(rtype));
            }
            else
            {
                MONO_EMIT_NEW_PCONST(cfg, dreg, null);
            }
        }

        private void MONO_EMIT_NEW_ICONST(IMethodContext cfg, IRegister dr, int imm)
        {
            var inst = MONO_INST_NEW((cfg), InstructionCode.OP_ICONST);
            inst.Destination = dr;
            inst.set_inst_c0(imm);

            MONO_ADD_INS(cfg.CurrentBasicBlock, inst);
        }

        private bool mini_type_var_is_vt(IMethodContext cfg, TypeReference rtype)
        {
            throw new NotImplementedException();
        }

        private void MONO_EMIT_NEW_PCONST(IMethodContext cfg, IRegister dr, IInstruction val)
        {
            var inst = MONO_INST_NEW(cfg, InstructionCode.OP_ICONST);
            inst.Destination = dr;
            inst.set_inst_p0(val);
            inst.StackType = StackType.STACK_PTR;
            MONO_ADD_INS((cfg).CurrentBasicBlock, inst);
        }
    }
}