using System;
using Mono.Cecil;
using Zenos.Core;
using Zenos.Framework;

namespace Zenos.Stages
{
    public class X86 : Architecture
    {
        private readonly Emitter _emitter;
        const int FLOAT_PARAM_REGS = 0;

        private static readonly IRegister X86_EAX = new X86Register(X86_Reg_No.X86_EAX);
        private static readonly IRegister X86_ECX = new X86Register(X86_Reg_No.X86_ECX);
        private static readonly IRegister X86_NREG = new X86Register(X86_Reg_No.X86_NREG);
        private static readonly IRegister X86_EDX = new X86Register(X86_Reg_No.X86_EDX);
        


        private static readonly IRegister[] thiscall_param_regs = { X86_ECX, X86_NREG };
        private bool mono_do_x86_stack_align = true;
        private uint MONO_ARCH_FRAME_ALIGNMENT = 16;


        public X86(Emitter emitter)
        {
            _emitter = emitter;
        }

        public override string Name
        {
            get { return "X86"; }
        }

        public override CallInfo get_call_info(object gsctx, MethodDefinition method)
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
                        cinfo.ret.reg = X86_EAX;
                        break;
                    case MetadataType.UInt64:
                    case MetadataType.Int64:
                        cinfo.ret.storage = ArgStorage.ArgInIReg;
                        cinfo.ret.reg = X86_EAX;
                        cinfo.ret.is_pair = true;
                        break;
                    case MetadataType.Single:
                        cinfo.ret.storage = ArgStorage.ArgOnFloatFpStack;
                        break;
                    case MetadataType.Double:
                        cinfo.ret.storage = ArgStorage.ArgOnDoubleFpStack;
                        break;
                    case MetadataType.GenericInstance:
                        if (!ret_type.mono_type_generic_inst_is_valuetype())
                        {
                            cinfo.ret.storage = ArgStorage.ArgInIReg;
                            cinfo.ret.reg = X86_EAX;
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
                        var tmpParamRegs = new IRegister[0];

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
                 (sig.Parameters.Count > 0 && mini_type_get_underlying_type(gsctx, sig.Parameters[0].ParameterType).MONO_TYPE_IS_REFERENCE()))
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
                IRegister[] temp = null;
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
                    IRegister[] temp = null;
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
                        if (!ptype.mono_type_generic_inst_is_valuetype())
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

            if (!sig.IsPInvokeImpl && (sig.CallingConvention == MethodCallingConvention.VarArg) && (n > 0) &&
                (false /*sig.sentinelpos == sig.Parameters.Count*/))
            {
                fr = FLOAT_PARAM_REGS;

                /* Emit the signature cookie just before the implicit arguments */
                add_general(ref gr, ref param_regs, ref stack_size, cinfo.sig_cookie);
            }

            if (mono_do_x86_stack_align && (stack_size % MONO_ARCH_FRAME_ALIGNMENT) != 0)
            {
                cinfo.need_stack_align = true;
                cinfo.stack_align_amount = MONO_ARCH_FRAME_ALIGNMENT - (stack_size % MONO_ARCH_FRAME_ALIGNMENT);
                stack_size += cinfo.stack_align_amount;
            }

            if (cinfo.vtype_retaddr)
            {
                /* if the function returns a struct on stack, the called method already does a ret $0x4 */
                cinfo.callee_stack_pop = 4;
            }

            cinfo.stack_usage = stack_size;
            cinfo.reg_usage = gr;
            cinfo.freg_usage = fr;
            return cinfo;
        }

        public override void mono_arch_emit_setret(IMethodContext cfg, MethodDefinition method, IInstruction instruction)
        {
            var ret = mini_type_get_underlying_type(cfg, method.ReturnType);

            if (!ret.IsByReference)
            {
                if (ret.MetadataType == MetadataType.Single)
                {
                    if (COMPILE_LLVM(cfg))
                        _emitter.MONO_EMIT_NEW_UNALU(cfg, InstructionCode.OP_FMOVE, cfg.ReturnType.Destination, instruction.Destination);
                    /* Nothing to do */
                    return;
                }
                
                if (ret.MetadataType == MetadataType.Double)
                {
                    if (COMPILE_LLVM(cfg))
                        _emitter.MONO_EMIT_NEW_UNALU(cfg, InstructionCode.OP_FMOVE, cfg.ReturnType.Destination, instruction.Destination);
                    /* Nothing to do */
                    return;
                }
                
                if (ret.MetadataType == MetadataType.Int64 || ret.MetadataType == MetadataType.UInt64)
                {
                    if (COMPILE_LLVM(cfg))
                        _emitter.MONO_EMIT_NEW_UNALU(cfg, InstructionCode.OP_LMOVE, cfg.ReturnType.Destination, instruction.Destination);
                    else
                    {
                        //TODO: need to investigave if this adding to the reg is correct or not.
                        _emitter.MONO_EMIT_NEW_UNALU(cfg, InstructionCode.OP_MOVE, X86_EAX, new Register(instruction.Destination.Id + 1));
                        _emitter.MONO_EMIT_NEW_UNALU(cfg, InstructionCode.OP_MOVE, X86_EDX, new Register(instruction.Destination.Id + 2));
                    }
                    return;
                }
            }

            _emitter.MONO_EMIT_NEW_UNALU(cfg, InstructionCode.OP_MOVE, cfg.ReturnType.Destination, instruction.Destination);
        }

        private bool COMPILE_LLVM(IMethodContext cfg)
        {
            return false;
        }

        private void add_float(ref uint fr, ref uint stackSize, ArgInfo ainfo, bool p3)
        {
            throw new NotImplementedException();
        }

        private void add_general_pair(ref uint gr, ref IRegister[] paramRegs, ref uint stackSize, ArgInfo ainfo)
        {
            throw new NotImplementedException();
        }

        private void add_general(ref uint gr, ref IRegister[] paramRegs, ref uint stackSize, ArgInfo ainfo)
        {
            ainfo.offset = stackSize;

            if (paramRegs == null || paramRegs[gr] == X86_NREG)
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

        private void add_valuetype(object gsctx, MethodDefinition sig, ArgInfo ainfo, TypeReference type, bool is_return, ref uint gr, IRegister[] param_regs, ref uint fr, ref uint stack_size)
        {
            throw new NotImplementedException();
        }

        private IRegister[] callconv_param_regs(MethodDefinition sig)
        {
            if (sig.IsPInvokeImpl)
                return new IRegister[0];

            switch (sig.CallingConvention)
            {
                case MethodCallingConvention.ThisCall:
                    return thiscall_param_regs;
                default:
                    return new IRegister[0];
            }
        }
    }

    internal class X86Register : Register
    {
        public X86Register(X86_Reg_No id) 
            : base((int)id)
        {
        }
    }

    public abstract class Architecture : IArchitecture
    {
        public abstract string Name { get; }
    

        public abstract CallInfo get_call_info(object gsctx, MethodDefinition method);
        public abstract void mono_arch_emit_setret(IMethodContext context, MethodDefinition method, IInstruction instruction);


        protected TypeReference mini_type_get_underlying_type(object gsctx, TypeReference type)
        {
            type = mini_native_type_replace_type(type);

            if (type.IsByReference)
                return mono_defaults.int_class;

            if (!type.IsByReference && (type.MetadataType == MetadataType.Var || type.MetadataType == MetadataType.MVar) &&
                mini_is_gsharedvt_type_gsctx(gsctx, type))
                return type;

            return mini_get_basic_type_from_generic(gsctx, type.Resolve().mono_type_get_underlying_type());
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

        protected bool mini_is_gsharedvt_type_gsctx(object gsctx, TypeReference retType)
        {
            throw new NotImplementedException();
        }

        private TypeReference mini_native_type_replace_type(TypeReference type)
        {
            return type;
        }

    }
}