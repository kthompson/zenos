#define TARGET_AMD64
#define TARGET_X86

namespace Zenos.Framework
{
    public enum InstructionCode
    {
        //First instructions codes map directly to Cecil's for easy conversion
        CilNop,
        CilBreak,
        CilLdarg_0,
        CilLdarg_1,
        CilLdarg_2,
        CilLdarg_3,
        CilLdloc_0,
        CilLdloc_1,
        CilLdloc_2,
        CilLdloc_3,
        CilStloc_0,
        CilStloc_1,
        CilStloc_2,
        CilStloc_3,
        CilLdarg_S,
        CilLdarga_S,
        CilStarg_S,
        CilLdloc_S,
        CilLdloca_S,
        CilStloc_S,
        CilLdnull,
        CilLdc_I4_M1,
        CilLdc_I4_0,
        CilLdc_I4_1,
        CilLdc_I4_2,
        CilLdc_I4_3,
        CilLdc_I4_4,
        CilLdc_I4_5,
        CilLdc_I4_6,
        CilLdc_I4_7,
        CilLdc_I4_8,
        CilLdc_I4_S,
        CilLdc_I4,
        CilLdc_I8,
        CilLdc_R4,
        CilLdc_R8,
        CilDup,
        CilPop,
        CilJmp,
        CilCall,
        CilCalli,
        CilRet,
        CilBr_S,
        CilBrfalse_S,
        CilBrtrue_S,
        CilBeq_S,
        CilBge_S,
        CilBgt_S,
        CilBle_S,
        CilBlt_S,
        CilBne_Un_S,
        CilBge_Un_S,
        CilBgt_Un_S,
        CilBle_Un_S,
        CilBlt_Un_S,
        CilBr,
        CilBrfalse,
        CilBrtrue,
        CilBeq,
        CilBge,
        CilBgt,
        CilBle,
        CilBlt,
        CilBne_Un,
        CilBge_Un,
        CilBgt_Un,
        CilBle_Un,
        CilBlt_Un,
        CilSwitch,
        CilLdind_I1,
        CilLdind_U1,
        CilLdind_I2,
        CilLdind_U2,
        CilLdind_I4,
        CilLdind_U4,
        CilLdind_I8,
        CilLdind_I,
        CilLdind_R4,
        CilLdind_R8,
        CilLdind_Ref,
        CilStind_Ref,
        CilStind_I1,
        CilStind_I2,
        CilStind_I4,
        CilStind_I8,
        CilStind_R4,
        CilStind_R8,
        CilAdd,
        CilSub,
        CilMul,
        CilDiv,
        CilDiv_Un,
        CilRem,
        CilRem_Un,
        CilAnd,
        CilOr,
        CilXor,
        CilShl,
        CilShr,
        CilShr_Un,
        CilNeg,
        CilNot,
        CilConv_I1,
        CilConv_I2,
        CilConv_I4,
        CilConv_I8,
        CilConv_R4,
        CilConv_R8,
        CilConv_U4,
        CilConv_U8,
        CilCallvirt,
        CilCpobj,
        CilLdobj,
        CilLdstr,
        CilNewobj,
        CilCastclass,
        CilIsinst,
        CilConv_R_Un,
        CilUnbox,
        CilThrow,
        CilLdfld,
        CilLdflda,
        CilStfld,
        CilLdsfld,
        CilLdsflda,
        CilStsfld,
        CilStobj,
        CilConv_Ovf_I1_Un,
        CilConv_Ovf_I2_Un,
        CilConv_Ovf_I4_Un,
        CilConv_Ovf_I8_Un,
        CilConv_Ovf_U1_Un,
        CilConv_Ovf_U2_Un,
        CilConv_Ovf_U4_Un,
        CilConv_Ovf_U8_Un,
        CilConv_Ovf_I_Un,
        CilConv_Ovf_U_Un,
        CilBox,
        CilNewarr,
        CilLdlen,
        CilLdelema,
        CilLdelem_I1,
        CilLdelem_U1,
        CilLdelem_I2,
        CilLdelem_U2,
        CilLdelem_I4,
        CilLdelem_U4,
        CilLdelem_I8,
        CilLdelem_I,
        CilLdelem_R4,
        CilLdelem_R8,
        CilLdelem_Ref,
        CilStelem_I,
        CilStelem_I1,
        CilStelem_I2,
        CilStelem_I4,
        CilStelem_I8,
        CilStelem_R4,
        CilStelem_R8,
        CilStelem_Ref,
        CilLdelem_Any,
        CilStelem_Any,
        CilUnbox_Any,
        CilConv_Ovf_I1,
        CilConv_Ovf_U1,
        CilConv_Ovf_I2,
        CilConv_Ovf_U2,
        CilConv_Ovf_I4,
        CilConv_Ovf_U4,
        CilConv_Ovf_I8,
        CilConv_Ovf_U8,
        CilRefanyval,
        CilCkfinite,
        CilMkrefany,
        CilLdtoken,
        CilConv_U2,
        CilConv_U1,
        CilConv_I,
        CilConv_Ovf_I,
        CilConv_Ovf_U,
        CilAdd_Ovf,
        CilAdd_Ovf_Un,
        CilMul_Ovf,
        CilMul_Ovf_Un,
        CilSub_Ovf,
        CilSub_Ovf_Un,
        CilEndfinally,
        CilLeave,
        CilLeave_S,
        CilStind_I,
        CilConv_U,
        CilArglist,
        CilCeq,
        CilCgt,
        CilCgt_Un,
        CilClt,
        CilClt_Un,
        CilLdftn,
        CilLdvirtftn,
        CilLdarg,
        CilLdarga,
        CilStarg,
        CilLdloc,
        CilLdloca,
        CilStloc,
        CilLocalloc,
        CilEndfilter,
        CilUnaligned,
        CilVolatile,
        CilTail,
        CilInitobj,
        CilConstrained,
        CilCpblk,
        CilInitblk,
        CilNo,
        CilRethrow,
        CilSizeof,
        CilRefanytype,
        CilReadonly,
        //the above instructions map directly to the Mono.Cecil.Cil enum

        IrFirst = 1000,

        OP_LOAD,
        OP_LDADDR,
        OP_STORE,
        OP_NOP,
        OP_HARD_NOP,
        OP_RELAXED_NOP,
        OP_PHI,
        OP_FPHI,
        OP_VPHI,
        OP_COMPARE,
        OP_COMPARE_IMM,
        OP_FCOMPARE,
        OP_LCOMPARE,
        OP_ICOMPARE,
        OP_ICOMPARE_IMM,
        OP_LCOMPARE_IMM,
        OP_LOCAL,
        OP_ARG,
        /* inst_imm contains the local index */
        OP_GSHAREDVT_LOCAL,
        OP_GSHAREDVT_ARG_REGOFFSET,
        /*
         * Represents passing a valuetype argument which has not been decomposed yet.
         * inst_p0 points to the call.
         */
        OP_OUTARG_VT,
        OP_OUTARG_VTRETADDR,
        OP_SETRET,
        OP_SETFRET,
        OP_SETLRET,
        OP_LOCALLOC,
        OP_LOCALLOC_IMM,
        OP_CHECK_THIS,
        OP_SEQ_POINT,
        OP_IMPLICIT_EXCEPTION,

        OP_VOIDCALL,
        OP_VOIDCALLVIRT,
        OP_VOIDCALL_REG,
        OP_VOIDCALL_MEMBASE,
        OP_CALL,
        OP_CALL_REG,
        OP_CALL_MEMBASE,
        OP_CALLVIRT,
        OP_FCALL,
        OP_FCALLVIRT,
        OP_FCALL_REG,
        OP_FCALL_MEMBASE,
        OP_LCALL,
        OP_LCALLVIRT,
        OP_LCALL_REG,
        OP_LCALL_MEMBASE,
        OP_VCALL,
        OP_VCALLVIRT,
        OP_VCALL_REG,
        OP_VCALL_MEMBASE,
        /* Represents the decomposed vcall which doesn't return a vtype no more */
        OP_VCALL2,
        OP_VCALL2_REG,
        OP_VCALL2_MEMBASE,
        OP_DYN_CALL,

        OP_ICONST,
        OP_I8CONST,
        OP_R4CONST,
        OP_R8CONST,
        OP_REGVAR,
        OP_REGOFFSET,
        OP_VTARG_ADDR,
        OP_LABEL,
        OP_SWITCH,
        OP_THROW,
        OP_RETHROW,

        /*
         * Vararg calls are implemented as follows:
         * - the caller emits a hidden argument just before the varargs argument. this
         *   'signature cookie' argument contains the signature describing the the call.
         * - all implicit arguments are passed in memory right after the signature cookie, i.e.
         *   the stack will look like this:
         *   <argn>
         *   ..
         *   <arg1>
         *   <sig cookie>
         * - the OP_ARGLIST opcode in the callee computes the address of the sig cookie argument
         *   on the stack and saves it into its sreg1.
         * - mono_ArgIterator_Setup receives this value and uses it to find the signature and
         *   the arguments.
         */
        OP_ARGLIST,

        /* MONO_IS_STORE_MEMBASE depends on the order here */
        OP_STORE_MEMBASE_REG,
        OP_STOREI1_MEMBASE_REG,
        OP_STOREI2_MEMBASE_REG,
        OP_STOREI4_MEMBASE_REG,
        OP_STOREI8_MEMBASE_REG,
        OP_STORER4_MEMBASE_REG,
        OP_STORER8_MEMBASE_REG,

#if TARGET_X86 || TARGET_AMD64
        OP_STOREX_MEMBASE_REG,
        OP_STOREX_ALIGNED_MEMBASE_REG,
        OP_STOREX_NTA_MEMBASE_REG,
#endif

        OP_STORE_MEMBASE_IMM,
        OP_STOREI1_MEMBASE_IMM,
        OP_STOREI2_MEMBASE_IMM,
        OP_STOREI4_MEMBASE_IMM,
        OP_STOREI8_MEMBASE_IMM,
        OP_STOREX_MEMBASE,
        OP_STOREV_MEMBASE,

        /* MONO_IS_LOAD_MEMBASE depends on the order here */
        OP_LOAD_MEMBASE,
        OP_LOADI1_MEMBASE,
        OP_LOADU1_MEMBASE,
        OP_LOADI2_MEMBASE,
        OP_LOADU2_MEMBASE,
        OP_LOADI4_MEMBASE,
        OP_LOADU4_MEMBASE,
        OP_LOADI8_MEMBASE,
        OP_LOADR4_MEMBASE,
        OP_LOADR8_MEMBASE,

        OP_LOADX_MEMBASE,

#if TARGET_X86 || TARGET_AMD64
        OP_LOADX_ALIGNED_MEMBASE,
#endif

        OP_LOADV_MEMBASE,

        /* indexed loads: dreg = load at (sreg1 + sreg2)*/
        OP_LOAD_MEMINDEX,
        OP_LOADI1_MEMINDEX,
        OP_LOADU1_MEMINDEX,
        OP_LOADI2_MEMINDEX,
        OP_LOADU2_MEMINDEX,
        OP_LOADI4_MEMINDEX,
        OP_LOADU4_MEMINDEX,
        OP_LOADI8_MEMINDEX,
        OP_LOADR4_MEMINDEX,
        OP_LOADR8_MEMINDEX,
        /* indexed stores: store sreg1 at (destbasereg + sreg2) */
        /* MONO_IS_STORE_MEMINDEX depends on the order here */
        OP_STORE_MEMINDEX,
        OP_STOREI1_MEMINDEX,
        OP_STOREI2_MEMINDEX,
        OP_STOREI4_MEMINDEX,
        OP_STOREI8_MEMINDEX,
        OP_STORER4_MEMINDEX,
        OP_STORER8_MEMINDEX,

        OP_LOAD_MEM,
        OP_LOADU1_MEM,
        OP_LOADU2_MEM,
        OP_LOADI4_MEM,
        OP_LOADU4_MEM,
        OP_LOADI8_MEM,
        OP_STORE_MEM_IMM,

        OP_MOVE,
        OP_LMOVE,
        OP_FMOVE,
        OP_VMOVE,

        OP_VZERO,

        OP_ADD_IMM,
        OP_SUB_IMM,
        OP_MUL_IMM,
        OP_DIV_IMM,
        OP_DIV_UN_IMM,
        OP_REM_IMM,
        OP_REM_UN_IMM,
        OP_AND_IMM,
        OP_OR_IMM,
        OP_XOR_IMM,
        OP_SHL_IMM,
        OP_SHR_IMM,
        OP_SHR_UN_IMM,

        OP_BR,
        OP_JMP,
        /* Same as OP_JMP, but the passing of arguments is done similarly to calls */
        OP_TAILCALL,
        OP_BREAK,

        OP_CEQ,
        OP_CGT,
        OP_CGT_UN,
        OP_CLT,
        OP_CLT_UN,

        /* exceptions: must be in the same order as the matching CEE_ branch opcodes */
        OP_COND_EXC_EQ,
        OP_COND_EXC_GE,
        OP_COND_EXC_GT,
        OP_COND_EXC_LE,
        OP_COND_EXC_LT,
        OP_COND_EXC_NE_UN,
        OP_COND_EXC_GE_UN,
        OP_COND_EXC_GT_UN,
        OP_COND_EXC_LE_UN,
        OP_COND_EXC_LT_UN,

        OP_COND_EXC_OV,
        OP_COND_EXC_NO,
        OP_COND_EXC_C,
        OP_COND_EXC_NC,

        OP_COND_EXC_IEQ,
        OP_COND_EXC_IGE,
        OP_COND_EXC_IGT,
        OP_COND_EXC_ILE,
        OP_COND_EXC_ILT,
        OP_COND_EXC_INE_UN,
        OP_COND_EXC_IGE_UN,
        OP_COND_EXC_IGT_UN,
        OP_COND_EXC_ILE_UN,
        OP_COND_EXC_ILT_UN,

        OP_COND_EXC_IOV,
        OP_COND_EXC_INO,
        OP_COND_EXC_IC,
        OP_COND_EXC_INC,

        /* 64 bit opcodes: must be in the same order as the matching CEE_ opcodes: binops_op_map */
        OP_LADD,
        OP_LSUB,
        OP_LMUL,
        OP_LDIV,
        OP_LDIV_UN,
        OP_LREM,
        OP_LREM_UN,
        OP_LAND,
        OP_LOR,
        OP_LXOR,
        OP_LSHL,
        OP_LSHR,
        OP_LSHR_UN,

        /* 64 bit opcodes: must be in the same order as the matching CEE_ opcodes: unops_op_map */
        OP_LNEG,
        OP_LNOT,
        OP_LCONV_TO_I1,
        OP_LCONV_TO_I2,
        OP_LCONV_TO_I4,
        OP_LCONV_TO_I8,
        OP_LCONV_TO_R4,
        OP_LCONV_TO_R8,
        OP_LCONV_TO_U4,
        OP_LCONV_TO_U8,

        OP_LCONV_TO_U2,
        OP_LCONV_TO_U1,
        OP_LCONV_TO_I,
        OP_LCONV_TO_OVF_I,
        OP_LCONV_TO_OVF_U,

        OP_LADD_OVF,
        OP_LADD_OVF_UN,
        OP_LMUL_OVF,
        OP_LMUL_OVF_UN,
        OP_LSUB_OVF,
        OP_LSUB_OVF_UN,

        OP_LCONV_TO_OVF_I1_UN,
        OP_LCONV_TO_OVF_I2_UN,
        OP_LCONV_TO_OVF_I4_UN,
        OP_LCONV_TO_OVF_I8_UN,
        OP_LCONV_TO_OVF_U1_UN,
        OP_LCONV_TO_OVF_U2_UN,
        OP_LCONV_TO_OVF_U4_UN,
        OP_LCONV_TO_OVF_U8_UN,
        OP_LCONV_TO_OVF_I_UN,
        OP_LCONV_TO_OVF_U_UN,

        OP_LCONV_TO_OVF_I1,
        OP_LCONV_TO_OVF_U1,
        OP_LCONV_TO_OVF_I2,
        OP_LCONV_TO_OVF_U2,
        OP_LCONV_TO_OVF_I4,
        OP_LCONV_TO_OVF_U4,
        OP_LCONV_TO_OVF_I8,
        OP_LCONV_TO_OVF_U8,

        /* mono_decompose_long_opts () depends on the order here */
        OP_LCEQ,
        OP_LCGT,
        OP_LCGT_UN,
        OP_LCLT,
        OP_LCLT_UN,

        OP_LCONV_TO_R_UN,
        OP_LCONV_TO_U,

        OP_LADD_IMM,
        OP_LSUB_IMM,
        OP_LMUL_IMM,
        OP_LAND_IMM,
        OP_LOR_IMM,
        OP_LXOR_IMM,
        OP_LSHL_IMM,
        OP_LSHR_IMM,
        OP_LSHR_UN_IMM,
        OP_LDIV_IMM,
        OP_LDIV_UN_IMM,
        OP_LREM_IMM,
        OP_LREM_UN_IMM,

        /* mono_decompose_long_opts () depends on the order here */
        OP_LBEQ,
        OP_LBGE,
        OP_LBGT,
        OP_LBLE,
        OP_LBLT,
        OP_LBNE_UN,
        OP_LBGE_UN,
        OP_LBGT_UN,
        OP_LBLE_UN,
        OP_LBLT_UN,

        /* Variants of the original opcodes which take the two parts of the long as two arguments */
        OP_LCONV_TO_R8_2,
        OP_LCONV_TO_R4_2,
        OP_LCONV_TO_R_UN_2,
        OP_LCONV_TO_OVF_I4_2,

        /* 32 bit opcodes: must be in the same order as the matching CEE_ opcodes: binops_op_map */
        OP_IADD,
        OP_ISUB,
        OP_IMUL,
        OP_IDIV,
        OP_IDIV_UN,
        OP_IREM,
        OP_IREM_UN,
        OP_IAND,
        OP_IOR,
        OP_IXOR,
        OP_ISHL,
        OP_ISHR,
        OP_ISHR_UN,

        /* 32 bit opcodes: must be in the same order as the matching CEE_ opcodes: unops_op_map */
        OP_INEG,
        OP_INOT,
        OP_ICONV_TO_I1,
        OP_ICONV_TO_I2,
        OP_ICONV_TO_I4,
        OP_ICONV_TO_I8,
        OP_ICONV_TO_R4,
        OP_ICONV_TO_R8,
        OP_ICONV_TO_U4,
        OP_ICONV_TO_U8,

        OP_ICONV_TO_R_UN,
        OP_ICONV_TO_U,

        /* 32 bit opcodes: must be in the same order as the matching CEE_ opcodes: ovfops_op_map */
        OP_ICONV_TO_U2,
        OP_ICONV_TO_U1,
        OP_ICONV_TO_I,
        OP_ICONV_TO_OVF_I,
        OP_ICONV_TO_OVF_U,
        OP_IADD_OVF,
        OP_IADD_OVF_UN,
        OP_IMUL_OVF,
        OP_IMUL_OVF_UN,
        OP_ISUB_OVF,
        OP_ISUB_OVF_UN,

        /* 32 bit opcodes: must be in the same order as the matching CEE_ opcodes: ovf2ops_op_map */
        OP_ICONV_TO_OVF_I1_UN,
        OP_ICONV_TO_OVF_I2_UN,
        OP_ICONV_TO_OVF_I4_UN,
        OP_ICONV_TO_OVF_I8_UN,
        OP_ICONV_TO_OVF_U1_UN,
        OP_ICONV_TO_OVF_U2_UN,
        OP_ICONV_TO_OVF_U4_UN,
        OP_ICONV_TO_OVF_U8_UN,
        OP_ICONV_TO_OVF_I_UN,
        OP_ICONV_TO_OVF_U_UN,

        /* 32 bit opcodes: must be in the same order as the matching CEE_ opcodes: ovf3ops_op_map */
        OP_ICONV_TO_OVF_I1,
        OP_ICONV_TO_OVF_U1,
        OP_ICONV_TO_OVF_I2,
        OP_ICONV_TO_OVF_U2,
        OP_ICONV_TO_OVF_I4,
        OP_ICONV_TO_OVF_U4,
        OP_ICONV_TO_OVF_I8,
        OP_ICONV_TO_OVF_U8,

        OP_IADC,
        OP_IADC_IMM,
        OP_ISBB,
        OP_ISBB_IMM,
        OP_IADDCC,
        OP_ISUBCC,

        OP_IADD_IMM,
        OP_ISUB_IMM,
        OP_IMUL_IMM,
        OP_IDIV_IMM,
        OP_IDIV_UN_IMM,
        OP_IREM_IMM,
        OP_IREM_UN_IMM,
        OP_IAND_IMM,
        OP_IOR_IMM,
        OP_IXOR_IMM,
        OP_ISHL_IMM,
        OP_ISHR_IMM,
        OP_ISHR_UN_IMM,

        OP_ICEQ,
        OP_ICGT,
        OP_ICGT_UN,
        OP_ICLT,
        OP_ICLT_UN,

        OP_IBEQ,
        OP_IBGE,
        OP_IBGT,
        OP_IBLE,
        OP_IBLT,
        OP_IBNE_UN,
        OP_IBGE_UN,
        OP_IBGT_UN,
        OP_IBLE_UN,
        OP_IBLT_UN,

        OP_FBEQ,
        OP_FBGE,
        OP_FBGT,
        OP_FBLE,
        OP_FBLT,
        OP_FBNE_UN,
        OP_FBGE_UN,
        OP_FBGT_UN,
        OP_FBLE_UN,
        OP_FBLT_UN,

        /* float opcodes: must be in the same order as the matching CEE_ opcodes: binops_op_map */
        OP_FADD,
        OP_FSUB,
        OP_FMUL,
        OP_FDIV,
        OP_FDIV_UN,
        OP_FREM,
        OP_FREM_UN,

        /* float opcodes: must be in the same order as the matching CEE_ opcodes: unops_op_map */
        OP_FNEG,
        OP_FNOT,
        OP_FCONV_TO_I1,
        OP_FCONV_TO_I2,
        OP_FCONV_TO_I4,
        OP_FCONV_TO_I8,
        OP_FCONV_TO_R4,
        OP_FCONV_TO_R8,
        OP_FCONV_TO_U4,
        OP_FCONV_TO_U8,

        OP_FCONV_TO_U2,
        OP_FCONV_TO_U1,
        OP_FCONV_TO_I,
        OP_FCONV_TO_OVF_I,
        OP_FCONV_TO_OVF_U,

        OP_FADD_OVF,
        OP_FADD_OVF_UN,
        OP_FMUL_OVF,
        OP_FMUL_OVF_UN,
        OP_FSUB_OVF,
        OP_FSUB_OVF_UN,

        OP_FCONV_TO_OVF_I1_UN,
        OP_FCONV_TO_OVF_I2_UN,
        OP_FCONV_TO_OVF_I4_UN,
        OP_FCONV_TO_OVF_I8_UN,
        OP_FCONV_TO_OVF_U1_UN,
        OP_FCONV_TO_OVF_U2_UN,
        OP_FCONV_TO_OVF_U4_UN,
        OP_FCONV_TO_OVF_U8_UN,
        OP_FCONV_TO_OVF_I_UN,
        OP_FCONV_TO_OVF_U_UN,

        OP_FCONV_TO_OVF_I1,
        OP_FCONV_TO_OVF_U1,
        OP_FCONV_TO_OVF_I2,
        OP_FCONV_TO_OVF_U2,
        OP_FCONV_TO_OVF_I4,
        OP_FCONV_TO_OVF_U4,
        OP_FCONV_TO_OVF_I8,
        OP_FCONV_TO_OVF_U8,

        /* These do the comparison too */
        OP_FCEQ,
        OP_FCGT,
        OP_FCGT_UN,
        OP_FCLT,
        OP_FCLT_UN,

        OP_FCEQ_MEMBASE,
        OP_FCGT_MEMBASE,
        OP_FCGT_UN_MEMBASE,
        OP_FCLT_MEMBASE,
        OP_FCLT_UN_MEMBASE,

        OP_FCONV_TO_U,
        OP_CKFINITE,

        /* Return the low 32 bits of a double vreg */
        OP_FGETLOW32,
        /* Return the high 32 bits of a double vreg */
        OP_FGETHIGH32,

        OP_JUMP_TABLE,

        /* aot compiler */
        OP_AOTCONST,
        OP_PATCH_INFO,
        OP_GOT_ENTRY,

        /* exception related opcodes */
        OP_CALL_HANDLER,
        OP_START_HANDLER,
        OP_ENDFILTER,
        OP_ENDFINALLY,

        /* inline (long)int * (long)int */
        OP_BIGMUL,
        OP_BIGMUL_UN,
        OP_IMIN_UN,
        OP_IMAX_UN,
        OP_LMIN_UN,
        OP_LMAX_UN,

        OP_MIN,
        OP_MAX,

        OP_IMIN,
        OP_IMAX,
        OP_LMIN,
        OP_LMAX,

        /* opcodes most architecture have */
        OP_ADC,
        OP_ADC_IMM,
        OP_SBB,
        OP_SBB_IMM,
        OP_ADDCC,
        OP_ADDCC_IMM,
        OP_SUBCC,
        OP_SUBCC_IMM,
        OP_BR_REG,
        OP_SEXT_I1,
        OP_SEXT_I2,
        OP_SEXT_I4,
        OP_ZEXT_I1,
        OP_ZEXT_I2,
        OP_ZEXT_I4,
        OP_CNE,
        OP_TRUNC_I4,
        /* to implement the upper half of long32 add and sub */
        OP_ADD_OVF_CARRY,
        OP_SUB_OVF_CARRY,
        OP_ADD_OVF_UN_CARRY,
        OP_SUB_OVF_UN_CARRY,

        /* instructions with explicit long arguments to deal with 64-bit ilp32 machines */
        OP_LADDCC,
        OP_LSUBCC,


        /* FP functions usually done by the CPU */
        OP_SIN,
        OP_COS,
        OP_ABS,
        OP_TAN,
        OP_ATAN,
        OP_SQRT,
        OP_ROUND,
        /* to optimize strings */
        OP_STRLEN,
        OP_NEWARR,
        OP_LDLEN,
        OP_BOUNDS_CHECK,
        /* get adress of element in a 2D array */
        OP_LDELEMA2D,
        /* inlined small memcpy with constant length */
        OP_MEMCPY,
        /* inlined small memset with constant length */
        OP_MEMSET,
        OP_SAVE_LMF,
        OP_RESTORE_LMF,

        /* write barrier */
        OP_CARD_TABLE_WBARRIER,

        /* arch-dep tls access */
        OP_TLS_GET,
        OP_TLS_GET_REG,

        OP_LOAD_GOTADDR,
        OP_DUMMY_USE,
        OP_DUMMY_STORE,
        OP_NOT_REACHED,
        OP_NOT_NULL,

        /* SIMD opcodes. */

#if TARGET_X86 || TARGET_AMD64

        OP_ADDPS,
        OP_DIVPS,
        OP_MULPS,
        OP_SUBPS,
        OP_MAXPS,
        OP_MINPS,
        OP_COMPPS,
        OP_ANDPS,
        OP_ANDNPS,
        OP_ORPS,
        OP_XORPS,
        OP_HADDPS,
        OP_HSUBPS,
        OP_ADDSUBPS,
        OP_DUPPS_LOW,
        OP_DUPPS_HIGH,

        OP_RSQRTPS,
        OP_SQRTPS,
        OP_RCPPS,

        OP_PSHUFLEW_HIGH,
        OP_PSHUFLEW_LOW,
        OP_PSHUFLED,
        OP_SHUFPS,
        OP_SHUFPD,

        OP_ADDPD,
        OP_DIVPD,
        OP_MULPD,
        OP_SUBPD,
        OP_MAXPD,
        OP_MINPD,
        OP_COMPPD,
        OP_ANDPD,
        OP_ANDNPD,
        OP_ORPD,
        OP_XORPD,
        OP_HADDPD,
        OP_HSUBPD,
        OP_ADDSUBPD,
        OP_DUPPD,

        OP_SQRTPD,

        OP_EXTRACT_MASK,

        OP_PAND,
        OP_POR,
        OP_PXOR,

        OP_PADDB,
        OP_PADDW,
        OP_PADDD,
        OP_PADDQ,

        OP_PSUBB,
        OP_PSUBW,
        OP_PSUBD,
        OP_PSUBQ,

        OP_PMAXB_UN,
        OP_PMAXW_UN,
        OP_PMAXD_UN,

        OP_PMAXB,
        OP_PMAXW,
        OP_PMAXD,

        OP_PAVGB_UN,
        OP_PAVGW_UN,

        OP_PMINB_UN,
        OP_PMINW_UN,
        OP_PMIND_UN,

        OP_PMINB,
        OP_PMINW,
        OP_PMIND,

        OP_PCMPEQB,
        OP_PCMPEQW,
        OP_PCMPEQD,
        OP_PCMPEQQ,

        OP_PCMPGTB,
        OP_PCMPGTW,
        OP_PCMPGTD,
        OP_PCMPGTQ,

        OP_PSUM_ABS_DIFF,

        OP_UNPACK_LOWB,
        OP_UNPACK_LOWW,
        OP_UNPACK_LOWD,
        OP_UNPACK_LOWQ,
        OP_UNPACK_LOWPS,
        OP_UNPACK_LOWPD,

        OP_UNPACK_HIGHB,
        OP_UNPACK_HIGHW,
        OP_UNPACK_HIGHD,
        OP_UNPACK_HIGHQ,
        OP_UNPACK_HIGHPS,
        OP_UNPACK_HIGHPD,

        OP_PACKW,
        OP_PACKD,

        OP_PACKW_UN,
        OP_PACKD_UN,

        OP_PADDB_SAT,
        OP_PADDB_SAT_UN,

        OP_PADDW_SAT,
        OP_PADDW_SAT_UN,

        OP_PSUBB_SAT,
        OP_PSUBB_SAT_UN,

        OP_PSUBW_SAT,
        OP_PSUBW_SAT_UN,

        OP_PMULW,
        OP_PMULD,
        OP_PMULQ,

        OP_PMULW_HIGH_UN,
        OP_PMULW_HIGH,

        /*SSE2 Shift ops must have the _reg version right after as code depends on this ordering.*/
        OP_PSHRW,
        OP_PSHRW_REG,

        OP_PSARW,
        OP_PSARW_REG,

        OP_PSHLW,
        OP_PSHLW_REG,

        OP_PSHRD,
        OP_PSHRD_REG,

        OP_PSHRQ,
        OP_PSHRQ_REG,

        OP_PSARD,
        OP_PSARD_REG,

        OP_PSHLD,
        OP_PSHLD_REG,

        OP_PSHLQ,
        OP_PSHLQ_REG,

        OP_EXTRACT_I4,
        OP_ICONV_TO_R8_RAW,

        OP_EXTRACT_I2,
        OP_EXTRACT_U2,
        OP_EXTRACT_I1,
        OP_EXTRACT_U1,
        OP_EXTRACT_R8,
        OP_EXTRACT_I8,

        /* Used by LLVM */
        OP_INSERT_I1,
        OP_INSERT_I4,
        OP_INSERT_I8,
        OP_INSERT_R4,
        OP_INSERT_R8,

        OP_INSERT_I2,

        OP_EXTRACTX_U2,

        /*these slow ops are modeled around the availability of a fast 2 bytes insert op*/
        /*insertx_u1_slow takes old value and new value as source regs */
        OP_INSERTX_U1_SLOW,
        /*insertx_i4_slow takes target xreg and new value as source regs */
        OP_INSERTX_I4_SLOW,

        OP_INSERTX_R4_SLOW,
        OP_INSERTX_R8_SLOW,
        OP_INSERTX_I8_SLOW,

        OP_FCONV_TO_R8_X,
        OP_XCONV_R8_TO_I4,
        OP_ICONV_TO_X,

        OP_EXPAND_I1,
        OP_EXPAND_I2,
        OP_EXPAND_I4,
        OP_EXPAND_R4,
        OP_EXPAND_I8,
        OP_EXPAND_R8,

        OP_PREFETCH_MEMBASE,

        OP_CVTDQ2PD,
        OP_CVTDQ2PS,
        OP_CVTPD2DQ,
        OP_CVTPD2PS,
        OP_CVTPS2DQ,
        OP_CVTPS2PD,
        OP_CVTTPD2DQ,
        OP_CVTTPS2DQ,

#endif

        OP_XMOVE,
        OP_XZERO,
        OP_XPHI,

        /* Atomic specific

            Note, OP_ATOMIC_ADD_IMM_NEW_I4 and
            OP_ATOMIC_ADD_NEW_I4 returns the new
            value compared to OP_ATOMIC_ADD_I4 that
            returns the old value.

            OP_ATOMIC_ADD_NEW_I4 is used by
            Interlocked::Increment and Interlocked:Decrement
            and atomic_add_i4 by Interlocked::Add
        */
        OP_ATOMIC_ADD_I4,
        OP_ATOMIC_ADD_NEW_I4,
        OP_ATOMIC_ADD_IMM_I4,
        OP_ATOMIC_ADD_IMM_NEW_I4,
        OP_ATOMIC_EXCHANGE_I4,

        OP_ATOMIC_ADD_I8,
        OP_ATOMIC_ADD_NEW_I8,
        OP_ATOMIC_ADD_IMM_I8,
        OP_ATOMIC_ADD_IMM_NEW_I8,
        OP_ATOMIC_EXCHANGE_I8,
        OP_MEMORY_BARRIER,

        OP_ATOMIC_CAS_I4,
        OP_ATOMIC_CAS_I8,

        /* Conditional move opcodes.
         * Must be in the same order as the matching CEE_B... opcodes
         * sreg2 will be assigned to dreg if the condition is true.
         * sreg1 should be equal to dreg and models the fact the instruction doesn't necessary
         * modify dreg. The sreg1==dreg condition could be violated by SSA, so the local
         * register allocator or the code generator should generate a mov dreg, sreg1 before
         * the cmov in those cases.
         * These opcodes operate on pointer sized values.
         */
        OP_CMOV_IEQ,
        OP_CMOV_IGE,
        OP_CMOV_IGT,
        OP_CMOV_ILE,
        OP_CMOV_ILT,
        OP_CMOV_INE_UN,
        OP_CMOV_IGE_UN,
        OP_CMOV_IGT_UN,
        OP_CMOV_ILE_UN,
        OP_CMOV_ILT_UN,

        OP_CMOV_LEQ,
        OP_CMOV_LGE,
        OP_CMOV_LGT,
        OP_CMOV_LLE,
        OP_CMOV_LLT,
        OP_CMOV_LNE_UN,
        OP_CMOV_LGE_UN,
        OP_CMOV_LGT_UN,
        OP_CMOV_LLE_UN,
        OP_CMOV_LLT_UN,

        /* Debugging support */
        /* 
         * Marks the start of the live range of the variable in inst_c0, that is the
         * first instruction where the variable has a value.
         */
        OP_LIVERANGE_START,
        /* 
         * Marks the end of the live range of the variable in inst_c0, that is the
         * first instruction where the variable no longer has a value.
         */
        OP_LIVERANGE_END,

        /* GC support */
        /*
         * mono_arch_output_basic_block () will set the backend.pc_offset field to the current pc
         * offset.
         */
        OP_GC_LIVENESS_DEF,
        OP_GC_LIVENESS_USE,

        /*
         * This marks the location inside a basic block where a GC tracked spill slot has been
         * defined. The spill slot is assumed to be alive until the end of the bblock.
         */
        OP_GC_SPILL_SLOT_LIVENESS_DEF,

        /*
         * This marks the location inside a basic block where a GC tracked param area slot has
         * been defined. The slot is assumed to be alive until the next call.
         */
        OP_GC_PARAM_SLOT_LIVENESS_DEF,

        /* Arch specific opcodes */
        /* #if defined(__native_client_codegen__) || defined(__native_client__) */
        /* We have to define these in terms of the TARGET defines, not NaCl defines */
        /* because genmdesc.pl doesn't have multiple defines per platform.          */
#if TARGET_AMD64 || TARGET_X86 || TARGET_ARM
        OP_NACL_GC_SAFE_POINT,
#endif

#if TARGET_X86 || TARGET_AMD64
        OP_X86_TEST_NULL,
        OP_X86_COMPARE_MEMBASE_REG,
        OP_X86_COMPARE_MEMBASE_IMM,
        OP_X86_COMPARE_MEM_IMM,
        OP_X86_COMPARE_MEMBASE8_IMM,
        OP_X86_COMPARE_REG_MEMBASE,
        OP_X86_INC_REG,
        OP_X86_INC_MEMBASE,
        OP_X86_DEC_REG,
        OP_X86_DEC_MEMBASE,
        OP_X86_ADD_MEMBASE_IMM,
        OP_X86_SUB_MEMBASE_IMM,
        OP_X86_AND_MEMBASE_IMM,
        OP_X86_OR_MEMBASE_IMM,
        OP_X86_XOR_MEMBASE_IMM,
        OP_X86_ADD_MEMBASE_REG,
        OP_X86_SUB_MEMBASE_REG,
        OP_X86_AND_MEMBASE_REG,
        OP_X86_OR_MEMBASE_REG,
        OP_X86_XOR_MEMBASE_REG,
        OP_X86_MUL_MEMBASE_REG,

        OP_X86_ADD_REG_MEMBASE,
        OP_X86_SUB_REG_MEMBASE,
        OP_X86_MUL_REG_MEMBASE,
        OP_X86_AND_REG_MEMBASE,
        OP_X86_OR_REG_MEMBASE,
        OP_X86_XOR_REG_MEMBASE,

        OP_X86_PUSH_MEMBASE,
        OP_X86_PUSH_IMM,
        OP_X86_PUSH,
        OP_X86_PUSH_OBJ,
        OP_X86_PUSH_GOT_ENTRY,
        OP_X86_LEA,
        OP_X86_LEA_MEMBASE,
        OP_X86_XCHG,
        OP_X86_FPOP,
        OP_X86_FP_LOAD_I8,
        OP_X86_FP_LOAD_I4,
        OP_X86_SETEQ_MEMBASE,
        OP_X86_SETNE_MEMBASE,
        OP_X86_FXCH,
    #endif

    #if TARGET_AMD64
        OP_AMD64_TEST_NULL,
        OP_AMD64_SET_XMMREG_R4,
        OP_AMD64_SET_XMMREG_R8,
        OP_AMD64_ICOMPARE_MEMBASE_REG,
        OP_AMD64_ICOMPARE_MEMBASE_IMM,
        OP_AMD64_ICOMPARE_REG_MEMBASE,
        OP_AMD64_COMPARE_MEMBASE_REG,
        OP_AMD64_COMPARE_MEMBASE_IMM,
        OP_AMD64_COMPARE_REG_MEMBASE,

        OP_AMD64_ADD_MEMBASE_REG,
        OP_AMD64_SUB_MEMBASE_REG,
        OP_AMD64_AND_MEMBASE_REG,
        OP_AMD64_OR_MEMBASE_REG,
        OP_AMD64_XOR_MEMBASE_REG,
        OP_AMD64_MUL_MEMBASE_REG,

        OP_AMD64_ADD_MEMBASE_IMM,
        OP_AMD64_SUB_MEMBASE_IMM,
        OP_AMD64_AND_MEMBASE_IMM,
        OP_AMD64_OR_MEMBASE_IMM,
        OP_AMD64_XOR_MEMBASE_IMM,
        OP_AMD64_MUL_MEMBASE_IMM,

        OP_AMD64_ADD_REG_MEMBASE,
        OP_AMD64_SUB_REG_MEMBASE,
        OP_AMD64_AND_REG_MEMBASE,
        OP_AMD64_OR_REG_MEMBASE,
        OP_AMD64_XOR_REG_MEMBASE,
        OP_AMD64_MUL_REG_MEMBASE,

        OP_AMD64_LOADI8_MEMINDEX,
        OP_AMD64_SAVE_SP_TO_LMF,
    #endif

    #if  __ppc__ || __powerpc__ || __ppc64__ || TARGET_POWERPC
    OP_PPC_SUBFIC,
    OP_PPC_SUBFZE,
    OP_CHECK_FINITE,
    #endif

    #if TARGET_ARM
    OP_ARM_RSBS_IMM,
    OP_ARM_RSC_IMM,
    #endif

    #if __sparc__ || sparc
    OP_SPARC_BRZ,
    OP_SPARC_BRLEZ,
    OP_SPARC_BRLZ,
    OP_SPARC_BRNZ,
    OP_SPARC_BRGZ,
    OP_SPARC_BRGEZ,
    OP_SPARC_COND_EXC_EQZ,
    OP_SPARC_COND_EXC_GEZ,
    OP_SPARC_COND_EXC_GTZ,
    OP_SPARC_COND_EXC_LEZ,
    OP_SPARC_COND_EXC_LTZ,
    OP_SPARC_COND_EXC_NEZ,
    #endif

    #if __s390__ || s390
    OP_S390_LOADARG,
    OP_S390_ARGREG,
    OP_S390_ARGPTR,
    OP_S390_STKARG,
    OP_S390_MOVE,
    OP_S390_SETF4RET,
    OP_S390_BKCHAIN,
    OP_S390_LADD,
    OP_S390_LADD_OVF,
    OP_S390_LADD_OVF_UN,
    OP_S390_LSUB,
    OP_S390_LSUB_OVF,
    OP_S390_LSUB_OVF_UN,
    OP_S390_LNEG,
    OP_S390_IADD_OVF,
    OP_S390_IADD_OVF_UN,
    OP_S390_ISUB_OVF,
    OP_S390_ISUB_OVF_UN,
    #endif

    #if __ia64__
    OP_IA64_LOAD,
    OP_IA64_LOADI1,
    OP_IA64_LOADU1,
    OP_IA64_LOADI2,
    OP_IA64_LOADU2,
    OP_IA64_LOADI4,
    OP_IA64_LOADU4,
    OP_IA64_LOADI8,
    OP_IA64_LOADU8,
    OP_IA64_LOADR4,
    OP_IA64_LOADR8,
    OP_IA64_STORE,
    OP_IA64_STOREI1,
    OP_IA64_STOREU1,
    OP_IA64_STOREI2,
    OP_IA64_STOREU2,
    OP_IA64_STOREI4,
    OP_IA64_STOREU4,
    OP_IA64_STOREI8,
    OP_IA64_STOREU8,
    OP_IA64_STORER4,
    OP_IA64_STORER8,

    OP_IA64_CMP4_EQ,
    OP_IA64_CMP4_NE,
    OP_IA64_CMP4_LE,
    OP_IA64_CMP4_LT,
    OP_IA64_CMP4_GE,
    OP_IA64_CMP4_GT,
    OP_IA64_CMP4_LE_UN,
    OP_IA64_CMP4_LT_UN,
    OP_IA64_CMP4_GE_UN,
    OP_IA64_CMP4_GT_UN,
    OP_IA64_CMP_EQ,
    OP_IA64_CMP_NE,
    OP_IA64_CMP_LE,
    OP_IA64_CMP_LT,
    OP_IA64_CMP_GE,
    OP_IA64_CMP_GT,
    OP_IA64_CMP_LT_UN,
    OP_IA64_CMP_GT_UN,
    OP_IA64_CMP_GE_UN,
    OP_IA64_CMP_LE_UN,

    OP_IA64_CMP4_EQ_IMM,
    OP_IA64_CMP4_NE_IMM,
    OP_IA64_CMP4_LE_IMM,
    OP_IA64_CMP4_LT_IMM,
    OP_IA64_CMP4_GE_IMM,
    OP_IA64_CMP4_GT_IMM,
    OP_IA64_CMP4_LE_UN_IMM,
    OP_IA64_CMP4_LT_UN_IMM,
    OP_IA64_CMP4_GE_UN_IMM,
    OP_IA64_CMP4_GT_UN_IMM,
    OP_IA64_CMP_EQ_IMM,
    OP_IA64_CMP_NE_IMM,
    OP_IA64_CMP_LE_IMM,
    OP_IA64_CMP_LT_IMM,
    OP_IA64_CMP_GE_IMM,
    OP_IA64_CMP_GT_IMM,
    OP_IA64_CMP_LT_UN_IMM,
    OP_IA64_CMP_GT_UN_IMM,
    OP_IA64_CMP_GE_UN_IMM,
    OP_IA64_CMP_LE_UN_IMM,

    OP_IA64_FCMP_EQ,
    OP_IA64_FCMP_NE,
    OP_IA64_FCMP_LE,
    OP_IA64_FCMP_LT,
    OP_IA64_FCMP_GE,
    OP_IA64_FCMP_GT,
    OP_IA64_FCMP_LT_UN,
    OP_IA64_FCMP_GT_UN,
    OP_IA64_FCMP_GE_UN,
    OP_IA64_FCMP_LE_UN,

    OP_IA64_BR_COND,
    OP_IA64_COND_EXC,
    OP_IA64_CSET,

    OP_IA64_STOREI1_MEMBASE_INC_REG,
    OP_IA64_STOREI2_MEMBASE_INC_REG,
    OP_IA64_STOREI4_MEMBASE_INC_REG,
    OP_IA64_STOREI8_MEMBASE_INC_REG,
    OP_IA64_STORER4_MEMBASE_INC_REG,
    OP_IA64_STORER8_MEMBASE_INC_REG,
    OP_IA64_LOADI1_MEMBASE_INC,
    OP_IA64_LOADU1_MEMBASE_INC,
    OP_IA64_LOADI2_MEMBASE_INC,
    OP_IA64_LOADU2_MEMBASE_INC,
    OP_IA64_LOADI4_MEMBASE_INC,
    OP_IA64_LOADU4_MEMBASE_INC,
    OP_IA64_LOADI8_MEMBASE_INC,
    OP_IA64_LOADR4_MEMBASE_INC,
    OP_IA64_LOADR8_MEMBASE_INC,
    #endif

    #if __mips__
    OP_MIPS_BEQ,
    OP_MIPS_BGEZ,
    OP_MIPS_BGTZ,
    OP_MIPS_BLEZ,
    OP_MIPS_BLTZ,
    OP_MIPS_BNE,
    OP_MIPS_CVTSD,
    OP_MIPS_FBEQ,
    OP_MIPS_FBGE,
    OP_MIPS_FBGE_UN,
    OP_MIPS_FBGT,
    OP_MIPS_FBGT_UN,
    OP_MIPS_FBLE,
    OP_MIPS_FBLE_UN,
    OP_MIPS_FBLT,
    OP_MIPS_FBLT_UN,
    OP_MIPS_FBNE,
    OP_MIPS_FBFALSE,
    OP_MIPS_FBTRUE,
    OP_MIPS_LWC1,
    OP_MIPS_MTC1S,
    OP_MIPS_MTC1S_2,
    OP_MIPS_MFC1S,
    OP_MIPS_MTC1D,
    OP_MIPS_MFC1D,
    OP_MIPS_NOP,
    OP_MIPS_SLTI,
    OP_MIPS_SLT,
    OP_MIPS_SLTIU,
    OP_MIPS_SLTU,

    OP_MIPS_COND_EXC_EQ,
    OP_MIPS_COND_EXC_GE,
    OP_MIPS_COND_EXC_GT,
    OP_MIPS_COND_EXC_LE,
    OP_MIPS_COND_EXC_LT,
    OP_MIPS_COND_EXC_NE_UN,
    OP_MIPS_COND_EXC_GE_UN,
    OP_MIPS_COND_EXC_GT_UN,
    OP_MIPS_COND_EXC_LE_UN,
    OP_MIPS_COND_EXC_LT_UN,

    OP_MIPS_COND_EXC_OV,
    OP_MIPS_COND_EXC_NO,
    OP_MIPS_COND_EXC_C,
    OP_MIPS_COND_EXC_NC,

    OP_MIPS_COND_EXC_IEQ,
    OP_MIPS_COND_EXC_IGE,
    OP_MIPS_COND_EXC_IGT,
    OP_MIPS_COND_EXC_ILE,
    OP_MIPS_COND_EXC_ILT,
    OP_MIPS_COND_EXC_INE_UN,
    OP_MIPS_COND_EXC_IGE_UN,
    OP_MIPS_COND_EXC_IGT_UN,
    OP_MIPS_COND_EXC_ILE_UN,
    OP_MIPS_COND_EXC_ILT_UN,

    OP_MIPS_COND_EXC_IOV,
    OP_MIPS_COND_EXC_INO,
    OP_MIPS_COND_EXC_IC,
    OP_MIPS_COND_EXC_INC,

    #endif

            /* Same as OUTARG_VT, but has a dreg */
    #if ENABLE_LLVM
    OP_LLVM_OUTARG_VT,
    #endif


    }
}