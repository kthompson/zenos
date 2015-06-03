﻿#define TARGET_AMD64
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

        ZirFirst = 1000,



        Unknown
    }
}