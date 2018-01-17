namespace Zenos.Framework


open Mono.Cecil
open Mono.Cecil.Cil

open Zenos.Core


type CilOffset = int


type CilCode =
| Nop = 0
| Break = 1
| Ldarg_0 = 2
| Ldarg_1 = 3
| Ldarg_2 = 4
| Ldarg_3 = 5
| Ldloc_0 = 6
| Ldloc_1 = 7
| Ldloc_2 = 8
| Ldloc_3 = 9
| Stloc_0 = 10
| Stloc_1 = 11
| Stloc_2 = 12
| Stloc_3 = 13
| Ldarg_S = 14
| Ldarga_S = 15
| Starg_S = 16
| Ldloc_S = 17
| Ldloca_S = 18
| Stloc_S = 19
| Ldnull = 20
| Ldc_I4_M1 = 21
| Ldc_I4_0 = 22
| Ldc_I4_1 = 23
| Ldc_I4_2 = 24
| Ldc_I4_3 = 25
| Ldc_I4_4 = 26
| Ldc_I4_5 = 27
| Ldc_I4_6 = 28
| Ldc_I4_7 = 29
| Ldc_I4_8 = 30
| Ldc_I4_S = 31
| Ldc_I4 = 32
| Ldc_I8 = 33
| Ldc_R4 = 34
| Ldc_R8 = 35
| Dup = 36
| Pop = 37
| Jmp = 38
| Call = 39
| Calli = 40
| Ret = 41
| Br_S = 42
| Brfalse_S = 43
| Brtrue_S = 44
| Beq_S = 45
| Bge_S = 46
| Bgt_S = 47
| Ble_S = 48
| Blt_S = 49
| Bne_Un_S = 50
| Bge_Un_S = 51
| Bgt_Un_S = 52
| Ble_Un_S = 53
| Blt_Un_S = 54
| Br = 55
| Brfalse = 56
| Brtrue = 57
| Beq = 58
| Bge = 59
| Bgt = 60
| Ble = 61
| Blt = 62
| Bne_Un = 63
| Bge_Un = 64
| Bgt_Un = 65
| Ble_Un = 66
| Blt_Un = 67
| Switch = 68
| Ldind_I1 = 69
| Ldind_U1 = 70
| Ldind_I2 = 71
| Ldind_U2 = 72
| Ldind_I4 = 73
| Ldind_U4 = 74
| Ldind_I8 = 75
| Ldind_I = 76
| Ldind_R4 = 77
| Ldind_R8 = 78
| Ldind_Ref = 79
| Stind_Ref = 80
| Stind_I1 = 81
| Stind_I2 = 82
| Stind_I4 = 83
| Stind_I8 = 84
| Stind_R4 = 85
| Stind_R8 = 86
| Add = 87
| Sub = 88
| Mul = 89
| Div = 90
| Div_Un = 91
| Rem = 92
| Rem_Un = 93
| And = 94
| Or = 95
| Xor = 96
| Shl = 97
| Shr = 98
| Shr_Un = 99
| Neg = 100
| Not = 101
| Conv_I1 = 102
| Conv_I2 = 103
| Conv_I4 = 104
| Conv_I8 = 105
| Conv_R4 = 106
| Conv_R8 = 107
| Conv_U4 = 108
| Conv_U8 = 109
| Callvirt = 110
| Cpobj = 111
| Ldobj = 112
| Ldstr = 113
| Newobj = 114
| Castclass = 115
| Isinst = 116
| Conv_R_Un = 117
| Unbox = 118
| Throw = 119
| Ldfld = 120
| Ldflda = 121
| Stfld = 122
| Ldsfld = 123
| Ldsflda = 124
| Stsfld = 125
| Stobj = 126
| Conv_Ovf_I1_Un = 127
| Conv_Ovf_I2_Un = 128
| Conv_Ovf_I4_Un = 129
| Conv_Ovf_I8_Un = 130
| Conv_Ovf_U1_Un = 131
| Conv_Ovf_U2_Un = 132
| Conv_Ovf_U4_Un = 133
| Conv_Ovf_U8_Un = 134
| Conv_Ovf_I_Un = 135
| Conv_Ovf_U_Un = 136
| Box = 137
| Newarr = 138
| Ldlen = 139
| Ldelema = 140
| Ldelem_I1 = 141
| Ldelem_U1 = 142
| Ldelem_I2 = 143
| Ldelem_U2 = 144
| Ldelem_I4 = 145
| Ldelem_U4 = 146
| Ldelem_I8 = 147
| Ldelem_I = 148
| Ldelem_R4 = 149
| Ldelem_R8 = 150
| Ldelem_Ref = 151
| Stelem_I = 152
| Stelem_I1 = 153
| Stelem_I2 = 154
| Stelem_I4 = 155
| Stelem_I8 = 156
| Stelem_R4 = 157
| Stelem_R8 = 158
| Stelem_Ref = 159
| Ldelem_Any = 160
| Stelem_Any = 161
| Unbox_Any = 162
| Conv_Ovf_I1 = 163
| Conv_Ovf_U1 = 164
| Conv_Ovf_I2 = 165
| Conv_Ovf_U2 = 166
| Conv_Ovf_I4 = 167
| Conv_Ovf_U4 = 168
| Conv_Ovf_I8 = 169
| Conv_Ovf_U8 = 170
| Refanyval = 171
| Ckfinite = 172
| Mkrefany = 173
| Ldtoken = 174
| Conv_U2 = 175
| Conv_U1 = 176
| Conv_I = 177
| Conv_Ovf_I = 178
| Conv_Ovf_U = 179
| Add_Ovf = 180
| Add_Ovf_Un = 181
| Mul_Ovf = 182
| Mul_Ovf_Un = 183
| Sub_Ovf = 184
| Sub_Ovf_Un = 185
| Endfinally = 186
| Leave = 187
| Leave_S = 188
| Stind_I = 189
| Conv_U = 190
| Arglist = 191
| Ceq = 192
| Cgt = 193
| Cgt_Un = 194
| Clt = 195
| Clt_Un = 196
| Ldftn = 197
| Ldvirtftn = 198
| Ldarg = 199
| Ldarga = 200
| Starg = 201
| Ldloc = 202
| Ldloca = 203
| Stloc = 204
| Localloc = 205
| Endfilter = 206
| Unaligned = 207
| Volatile = 208
| Tail = 209
| Initobj = 210
| Constrained = 211
| Cpblk = 212
| Initblk = 213
| No = 214
| Rethrow = 215
| Sizeof = 216
| Refanytype = 217
| Readonly = 218

type ImmediateOperand =
| ImmediateByte of byte
| ImmediateSByte of sbyte
| ImmediateInt16 of int16
| ImmediateInt32 of int32
| ImmediateSingle of float32
| ImmediateDouble of float
| ImmediateInt64 of int64
| ImmediateString of string

type ZenosOpCode = 
| Load of ImmediateOperand
| Store of int
| Local
| LoadArgument
//| PushConstant
//| PushArgument
//| Pop

type Operand =
| Immediate of ImmediateOperand

| LabelReference of string
| Register of Register
| Memory

| InstructionOperand of Instruction
| InstructionsOperand of Instruction list

// TODO: convert these to non-cecil types ?
| TypeReferenceOperand of TypeReference
| MethodReferenceOperand of MethodReference
| FieldReferenceOperand of FieldReference
| VariableOperand of VariableDefinition
| ParameterOperand of int
| CallSiteOperand of CallSite


and X86Code =
| Add of src:Operand * dest:Operand
| Move of src:Operand * dest:Operand
| Pop of src:Operand
| Push of src:Operand
| Syscall
| Xor of src:Operand * dest:Operand
| Nop

and Data =
| DataString of string
| DataFloat of float
| DataInt of int64

and CilInstruction = {Code:CilCode; Offset:CilOffset; Operand: Operand option}

and Instruction = 
| Zenos of OpCode:ZenosOpCode
| Cil of CilInstruction
| X86_64 of OpCode:X86Code
| Emit of byte list * string
| Data8 of Data list
| Data16 of Data list
| Data32 of Data list
| Data64 of Data list

module CecilOperand =
    let Create (value: obj) (instMap: Map<CilOffset, Instruction>) =
        match value with
        | null ->  None |> Ok
        | :? TypeReference as typ -> typ |> TypeReferenceOperand |> Some |> Ok
        | :? CallSite as typ -> typ |> CallSiteOperand |> Some |> Ok
        | :? MethodReference as typ -> typ |> MethodReferenceOperand |> Some |> Ok
        | :? FieldReference as typ -> typ |> FieldReferenceOperand |> Some |> Ok
        | :? string as typ -> typ |> ImmediateString |> Immediate |> Some |> Ok
        | :? sbyte as typ -> typ |> ImmediateSByte |> Immediate |> Some |> Ok
        | :? byte as typ -> typ |> ImmediateByte |> Immediate |> Some |> Ok
        | :? int as typ -> typ |> ImmediateInt32 |> Immediate |> Some |> Ok
        | :? int64 as i -> i |> ImmediateInt64 |> Immediate |> Some |> Ok
        | :? float32 as f -> f |> ImmediateSingle |> Immediate |> Some |> Ok
        | :? float as f -> f |> ImmediateDouble |> Immediate |> Some |> Ok
        | :? VariableDefinition as v -> v |> VariableOperand |> Some |> Ok
        | :? ParameterDefinition as parameter -> parameter.Index + 1 |> ParameterOperand |> Some |> Ok
        | :? Instruction as inst -> inst |> InstructionOperand |> Some |> Ok
        | :? Cil.Instruction as inst ->
            inst.Offset
            |> instMap.TryFind 
            |> Result.ofOption (sprintf "Instruction not found as offset %d" inst.Offset)
            |> Result.map (InstructionOperand >> Some)

        | :? List<Instruction> as typ -> typ |> InstructionsOperand |> Some |> Ok
        | value ->
            value.GetType().FullName
            |> sprintf "Expected operand type. Found: %s" 
            |> Error



type Label = Label of string

type SectionEntry =
| Label of Label
| SectionEntry of Label option * Instruction

type Directive =
| Global of Name:string
| Extern of Name:string
| Section of Name:string * SectionEntry list

type Listing = Listing of Directive list

module Instruction =
    let add src dest = Add (src, dest) |> X86_64
    let move src dest = Move (src, dest) |> X86_64
    let pop dest = Pop dest |> X86_64
    let push dest = Push dest |> X86_64
    let syscall = X86_64 Syscall
    let xor src dest = Xor (src, dest) |> X86_64
   