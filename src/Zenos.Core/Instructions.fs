namespace Zenos.Framework


open Mono.Cecil
open Mono.Cecil.Cil

open Zenos.Core


type CilOffset = int

type CilCode =
| Nop | Break 
| Ldarg_0 | Ldarg_1 | Ldarg_2 | Ldarg_3
| Ldloc_0 | Ldloc_1 | Ldloc_2 | Ldloc_3
| Stloc_0 | Stloc_1 | Stloc_2 | Stloc_3
| Ldarg_S of sbyte
| Ldarga_S of sbyte
| Starg_S 
| Ldloc_S | Ldloca_S
| Stloc_S
| Ldnull
| Ldc_I4_M1 | Ldc_I4_0 | Ldc_I4_1 | Ldc_I4_2 | Ldc_I4_3 | Ldc_I4_4
| Ldc_I4_5 | Ldc_I4_6 | Ldc_I4_7 | Ldc_I4_8
| Ldc_I4_S of sbyte
| Ldc_I4 of int | Ldc_I8 of int64 | Ldc_R4 of single | Ldc_R8 of double
| Dup | Pop
| Jmp 
| Call | Calli
| Ret
| Br_S of CilOffset | Br of CilOffset
| Brfalse_S of CilOffset | Brfalse of CilOffset
| Brtrue_S of CilOffset | Brtrue of CilOffset
| Beq_S of CilOffset | Beq of CilOffset
| Bge_S of CilOffset | Bge of CilOffset
| Bgt_S of CilOffset | Bgt of CilOffset
| Ble_S of CilOffset | Ble of CilOffset
| Blt_S of CilOffset | Blt of CilOffset
| Bne_Un_S of CilOffset | Bne_Un of CilOffset
| Bge_Un_S of CilOffset | Bge_Un of CilOffset
| Bgt_Un_S of CilOffset | Bgt_Un of CilOffset
| Ble_Un_S of CilOffset | Ble_Un of CilOffset
| Blt_Un_S of CilOffset | Blt_Un of CilOffset
| Switch of CilOffset list
| Ldind_I1 | Ldind_U1 | Ldind_I2 | Ldind_U2 | Ldind_I4 | Ldind_U4 | Ldind_I8
| Ldind_I | Ldind_R4 | Ldind_R8 | Ldind_Ref
| Stind_Ref | Stind_I1 | Stind_I2 | Stind_I4 | Stind_I8 | Stind_R4 | Stind_R8
| Add | Sub | Mul | Div | Div_Un
| Rem | Rem_Un
| And | Or | Xor
| Shl | Shr | Shr_Un
| Neg | Not
| Conv_I1 | Conv_I2 | Conv_I4 | Conv_I8 | Conv_R4 | Conv_R8 | Conv_U4 | Conv_U8
| Callvirt
| Cpobj
| Ldobj
| Ldstr
| Newobj
| Castclass
| Isinst
| Conv_R_Un
| Unbox
| Throw
| Ldfld
| Ldflda
| Stfld
| Ldsfld
| Ldsflda
| Stsfld
| Stobj
| Conv_Ovf_I1_Un | Conv_Ovf_I2_Un | Conv_Ovf_I4_Un | Conv_Ovf_I8_Un | Conv_Ovf_U1_Un
| Conv_Ovf_U2_Un | Conv_Ovf_U4_Un | Conv_Ovf_U8_Un | Conv_Ovf_I_Un | Conv_Ovf_U_Un
| Box
| Newarr
| Ldlen
| Ldelema
| Ldelem_I1
| Ldelem_U1
| Ldelem_I2
| Ldelem_U2
| Ldelem_I4
| Ldelem_U4
| Ldelem_I8
| Ldelem_I
| Ldelem_R4
| Ldelem_R8
| Ldelem_Ref
| Stelem_I
| Stelem_I1
| Stelem_I2
| Stelem_I4
| Stelem_I8
| Stelem_R4
| Stelem_R8
| Stelem_Ref
| Ldelem_Any
| Stelem_Any
| Unbox_Any
| Conv_Ovf_I1
| Conv_Ovf_U1
| Conv_Ovf_I2
| Conv_Ovf_U2
| Conv_Ovf_I4
| Conv_Ovf_U4
| Conv_Ovf_I8
| Conv_Ovf_U8
| Refanyval
| Ckfinite
| Mkrefany
| Ldtoken
| Conv_U2
| Conv_U1
| Conv_I
| Conv_Ovf_I
| Conv_Ovf_U
| Add_Ovf
| Add_Ovf_Un
| Mul_Ovf
| Mul_Ovf_Un
| Sub_Ovf
| Sub_Ovf_Un
| Endfinally
| Leave
| Leave_S
| Stind_I
| Conv_U
| Arglist
| Ceq
| Cgt
| Cgt_Un
| Clt
| Clt_Un
| Ldftn
| Ldvirtftn
| Ldarg
| Ldarga
| Starg
| Ldloc
| Ldloca
| Stloc
| Localloc
| Endfilter
| Unaligned
| Volatile
| Tail
| Initobj
| Constrained
| Cpblk
| Initblk
| No
| Rethrow
| Sizeof
| Refanytype
| Readonly

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

| OffsetOperand of CilOffset
| OffsetsOperand of CilOffset list

// TODO: convert these to non-cecil types ?
| TypeReferenceOperand of TypeReference
| MethodReferenceOperand of MethodReference
| FieldReferenceOperand of FieldReference
| VariableOperand of VariableDefinition
| ParameterOperand of int
| CallSiteOperand of CallSite


type X86Code =
| Add of src:Operand * dest:Operand
| Move of src:Operand * dest:Operand
| Pop of src:Operand
| Push of src:Operand
| Syscall
| Xor of src:Operand * dest:Operand
| Nop

type Data =
| DataString of string
| DataFloat of float
| DataInt of int64

type CilInstruction = {Code:CilCode; Offset:CilOffset}
type X86Instruction = {Bytes: byte list; Description: string }

type Instruction = 
| Zenos of OpCode:ZenosOpCode
| Cil of CilInstruction
| X86_64 of OpCode:X86Code
| Emit of CilOffset option * X86Instruction list
| Data8 of Data list
| Data16 of Data list
| Data32 of Data list
| Data64 of Data list


module Operand =
    let fromCecil (value: obj) =
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
        | :? Cil.Instruction as inst ->
            inst.Offset |> OffsetOperand |> Some |> Ok
        //| :? Array<Cil.Instruction> as typ ->
        //    typ |>
        //    List.map(fun x -> CilOffset(x.Offset)) |> InstructionsOperand |> Some |> Ok
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

module CilCode =
    let fromCecil (code: Mono.Cecil.Cil.Code) (operand: Operand option) : Result<CilCode, _> =
        let immOp () =
            match operand with
            | Some(Immediate imm) -> Ok imm
            | _ -> Error "not an immediate operand"

        let intOp =
            immOp
            >> Result.bind(function
            | ImmediateInt32 i -> Ok i
            | _ -> Error "not an int operand")

        let longOp  =
            immOp
            >> Result.bind(function
            | ImmediateInt64 i -> Ok i
            | _ -> Error "not a long operand")

        let sbyteOp  =
            immOp
            >> Result.bind(function
            | ImmediateSByte s -> Ok s
            | _ -> Error "not an sbyte operand")

        let singleOp =
            immOp
            >> Result.bind(function
            | ImmediateSingle s -> Ok s
            | _ -> Error "not a single float operand")

        let doubleOp  =
            immOp
            >> Result.bind(function
            | ImmediateDouble d -> Ok d
            | _ -> Error "not a single float operand")

        let paramOp () =
            match operand with
            | Some(ParameterOperand int) -> Ok (sbyte(int))
            | _ -> Error "not a parameter operand"
            
        let offsetOp () =
            match operand with
            | Some(OffsetOperand offset) -> Ok offset
            | _ -> Error "not an offset operand"


        let offsetsOp () =
            match operand with
            | Some(OffsetsOperand offsets) -> Ok offsets
            | _ -> Error "not an offsets operand"

        code
        |> LanguagePrimitives.EnumToValue
        |> function
        | 0 -> CilCode.Nop |> Ok
        | 1 -> Break |> Ok
        | 2 -> Ldarg_0 |> Ok
        | 3 -> Ldarg_1 |> Ok
        | 4 -> Ldarg_2 |> Ok
        | 5 -> Ldarg_3 |> Ok
        | 6 -> Ldloc_0 |> Ok
        | 7 -> Ldloc_1 |> Ok
        | 8 -> Ldloc_2 |> Ok
        | 9 -> Ldloc_3 |> Ok
        | 10 -> Stloc_0 |> Ok
        | 11 -> Stloc_1 |> Ok
        | 12 -> Stloc_2 |> Ok
        | 13 -> Stloc_3 |> Ok
        | 14 -> paramOp () |> Result.map Ldarg_S
        | 15 -> paramOp () |> Result.map Ldarga_S
        | 16 -> Starg_S |> Ok
        | 17 -> Ldloc_S |> Ok
        | 18 -> Ldloca_S |> Ok
        | 19 -> Stloc_S |> Ok
        | 20 -> Ldnull |> Ok
        | 21 -> Ldc_I4_M1 |> Ok
        | 22 -> Ldc_I4_0 |> Ok
        | 23 -> Ldc_I4_1 |> Ok
        | 24 -> Ldc_I4_2 |> Ok
        | 25 -> Ldc_I4_3 |> Ok
        | 26 -> Ldc_I4_4 |> Ok
        | 27 -> Ldc_I4_5 |> Ok
        | 28 -> Ldc_I4_6 |> Ok
        | 29 -> Ldc_I4_7 |> Ok
        | 30 -> Ldc_I4_8 |> Ok
        | 31 -> sbyteOp () |> Result.map Ldc_I4_S
        | 32 -> intOp () |> Result.map Ldc_I4
        | 33 -> longOp () |> Result.map Ldc_I8
        | 34 -> singleOp () |> Result.map Ldc_R4
        | 35 -> doubleOp () |> Result.map Ldc_R8
        | 36 -> Dup |> Ok
        | 37 -> CilCode.Pop |> Ok
        | 38 -> Jmp |> Ok
        | 39 -> Call |> Ok
        | 40 -> Calli |> Ok
        | 41 -> Ret |> Ok
        | 42 -> offsetOp () |> Result.map Br_S
        | 43 -> offsetOp () |> Result.map Brfalse_S
        | 44 -> offsetOp () |> Result.map Brtrue_S
        | 45 -> offsetOp () |> Result.map Beq_S
        | 46 -> offsetOp () |> Result.map Bge_S
        | 47 -> offsetOp () |> Result.map Bgt_S
        | 48 -> offsetOp () |> Result.map Ble_S
        | 49 -> offsetOp () |> Result.map Blt_S
        | 50 -> offsetOp () |> Result.map Bne_Un_S
        | 51 -> offsetOp () |> Result.map Bge_Un_S
        | 52 -> offsetOp () |> Result.map Bgt_Un_S
        | 53 -> offsetOp () |> Result.map Ble_Un_S
        | 54 -> offsetOp () |> Result.map Blt_Un_S
        | 55 -> offsetOp () |> Result.map Br
        | 56 -> offsetOp () |> Result.map Brfalse
        | 57 -> offsetOp () |> Result.map Brtrue
        | 58 -> offsetOp () |> Result.map Beq
        | 59 -> offsetOp () |> Result.map Bge
        | 60 -> offsetOp () |> Result.map Bgt
        | 61 -> offsetOp () |> Result.map Ble
        | 62 -> offsetOp () |> Result.map Blt
        | 63 -> offsetOp () |> Result.map Bne_Un
        | 64 -> offsetOp () |> Result.map Bge_Un
        | 65 -> offsetOp () |> Result.map Bgt_Un
        | 66 -> offsetOp () |> Result.map Ble_Un
        | 67 -> offsetOp () |> Result.map Blt_Un
        | 68 -> offsetsOp () |> Result.map Switch
        | 69 -> Ldind_I1 |> Ok
        | 70 -> Ldind_U1 |> Ok
        | 71 -> Ldind_I2 |> Ok
        | 72 -> Ldind_U2 |> Ok
        | 73 -> Ldind_I4 |> Ok
        | 74 -> Ldind_U4 |> Ok
        | 75 -> Ldind_I8 |> Ok
        | 76 -> Ldind_I |> Ok
        | 77 -> Ldind_R4 |> Ok
        | 78 -> Ldind_R8 |> Ok
        | 79 -> Ldind_Ref |> Ok
        | 80 -> Stind_Ref |> Ok
        | 81 -> Stind_I1 |> Ok
        | 82 -> Stind_I2 |> Ok
        | 83 -> Stind_I4 |> Ok
        | 84 -> Stind_I8 |> Ok
        | 85 -> Stind_R4 |> Ok
        | 86 -> Stind_R8 |> Ok
        | 87 -> CilCode.Add |> Ok
        | 88 -> Sub |> Ok
        | 89 -> Mul |> Ok
        | 90 -> Div |> Ok
        | 91 -> Div_Un |> Ok
        | 92 -> Rem |> Ok
        | 93 -> Rem_Un |> Ok
        | 94 -> And |> Ok
        | 95 -> Or |> Ok
        | 96 -> CilCode.Xor |> Ok
        | 97 -> Shl |> Ok
        | 98 -> Shr |> Ok
        | 99 -> Shr_Un |> Ok
        | 100 -> Neg |> Ok
        | 101 -> Not |> Ok
        | 102 -> Conv_I1 |> Ok
        | 103 -> Conv_I2 |> Ok
        | 104 -> Conv_I4 |> Ok
        | 105 -> Conv_I8 |> Ok
        | 106 -> Conv_R4 |> Ok
        | 107 -> Conv_R8 |> Ok
        | 108 -> Conv_U4 |> Ok
        | 109 -> Conv_U8 |> Ok
        | 110 -> Callvirt |> Ok
        | 111 -> Cpobj |> Ok
        | 112 -> Ldobj |> Ok
        | 113 -> Ldstr |> Ok
        | 114 -> Newobj |> Ok
        | 115 -> Castclass |> Ok
        | 116 -> Isinst |> Ok
        | 117 -> Conv_R_Un |> Ok
        | 118 -> Unbox |> Ok
        | 119 -> Throw |> Ok
        | 120 -> Ldfld |> Ok
        | 121 -> Ldflda |> Ok
        | 122 -> Stfld |> Ok
        | 123 -> Ldsfld |> Ok
        | 124 -> Ldsflda |> Ok
        | 125 -> Stsfld |> Ok
        | 126 -> Stobj |> Ok
        | 127 -> Conv_Ovf_I1_Un |> Ok
        | 128 -> Conv_Ovf_I2_Un |> Ok
        | 129 -> Conv_Ovf_I4_Un |> Ok
        | 130 -> Conv_Ovf_I8_Un |> Ok
        | 131 -> Conv_Ovf_U1_Un |> Ok
        | 132 -> Conv_Ovf_U2_Un |> Ok
        | 133 -> Conv_Ovf_U4_Un |> Ok
        | 134 -> Conv_Ovf_U8_Un |> Ok
        | 135 -> Conv_Ovf_I_Un |> Ok
        | 136 -> Conv_Ovf_U_Un |> Ok
        | 137 -> Box |> Ok
        | 138 -> Newarr |> Ok
        | 139 -> Ldlen |> Ok
        | 140 -> Ldelema |> Ok
        | 141 -> Ldelem_I1 |> Ok
        | 142 -> Ldelem_U1 |> Ok
        | 143 -> Ldelem_I2 |> Ok
        | 144 -> Ldelem_U2 |> Ok
        | 145 -> Ldelem_I4 |> Ok
        | 146 -> Ldelem_U4 |> Ok
        | 147 -> Ldelem_I8 |> Ok
        | 148 -> Ldelem_I |> Ok
        | 149 -> Ldelem_R4 |> Ok
        | 150 -> Ldelem_R8 |> Ok
        | 151 -> Ldelem_Ref |> Ok
        | 152 -> Stelem_I |> Ok
        | 153 -> Stelem_I1 |> Ok
        | 154 -> Stelem_I2 |> Ok
        | 155 -> Stelem_I4 |> Ok
        | 156 -> Stelem_I8 |> Ok
        | 157 -> Stelem_R4 |> Ok
        | 158 -> Stelem_R8 |> Ok
        | 159 -> Stelem_Ref |> Ok
        | 160 -> Ldelem_Any |> Ok
        | 161 -> Stelem_Any |> Ok
        | 162 -> Unbox_Any |> Ok
        | 163 -> Conv_Ovf_I1 |> Ok
        | 164 -> Conv_Ovf_U1 |> Ok
        | 165 -> Conv_Ovf_I2 |> Ok
        | 166 -> Conv_Ovf_U2 |> Ok
        | 167 -> Conv_Ovf_I4 |> Ok
        | 168 -> Conv_Ovf_U4 |> Ok
        | 169 -> Conv_Ovf_I8 |> Ok
        | 170 -> Conv_Ovf_U8 |> Ok
        | 171 -> Refanyval |> Ok
        | 172 -> Ckfinite |> Ok
        | 173 -> Mkrefany |> Ok
        | 174 -> Ldtoken |> Ok
        | 175 -> Conv_U2 |> Ok
        | 176 -> Conv_U1 |> Ok
        | 177 -> Conv_I |> Ok
        | 178 -> Conv_Ovf_I |> Ok
        | 179 -> Conv_Ovf_U |> Ok
        | 180 -> Add_Ovf |> Ok
        | 181 -> Add_Ovf_Un |> Ok
        | 182 -> Mul_Ovf |> Ok
        | 183 -> Mul_Ovf_Un |> Ok
        | 184 -> Sub_Ovf |> Ok
        | 185 -> Sub_Ovf_Un |> Ok
        | 186 -> Endfinally |> Ok
        | 187 -> Leave |> Ok
        | 188 -> Leave_S |> Ok
        | 189 -> Stind_I |> Ok
        | 190 -> Conv_U |> Ok
        | 191 -> Arglist |> Ok
        | 192 -> Ceq |> Ok
        | 193 -> Cgt |> Ok
        | 194 -> Cgt_Un |> Ok
        | 195 -> Clt |> Ok
        | 196 -> Clt_Un |> Ok
        | 197 -> Ldftn |> Ok
        | 198 -> Ldvirtftn |> Ok
        | 199 -> Ldarg |> Ok
        | 200 -> Ldarga |> Ok
        | 201 -> Starg |> Ok
        | 202 -> Ldloc |> Ok
        | 203 -> Ldloca |> Ok
        | 204 -> Stloc |> Ok
        | 205 -> Localloc |> Ok
        | 206 -> Endfilter |> Ok
        | 207 -> Unaligned |> Ok
        | 208 -> Volatile |> Ok
        | 209 -> Tail |> Ok
        | 210 -> Initobj |> Ok
        | 211 -> Constrained |> Ok
        | 212 -> Cpblk |> Ok
        | 213 -> Initblk |> Ok
        | 214 -> No |> Ok
        | 215 -> Rethrow |> Ok
        | 216 -> Sizeof |> Ok
        | 217 -> Refanytype |> Ok
        | 218 -> Readonly |> Ok
        | _ -> Error "invalid opcode" 

module Instruction =
    open Result 

    let add src dest = Add (src, dest) |> X86_64
    let move src dest = Move (src, dest) |> X86_64
    let pop dest = Pop dest |> X86_64
    let push dest = Push dest |> X86_64
    let syscall = X86_64 Syscall
    let xor src dest = Xor (src, dest) |> X86_64


    let fromCecil (inst: Cil.Instruction) : Result<Instruction, string> =
        result {
            let! op = Operand.fromCecil inst.Operand
            let! code = CilCode.fromCecil inst.OpCode.Code op

            return Cil {Code = code; Offset = inst.Offset}
        } 
   
        
module X86Instruction =
    let pushRax = {Bytes = [ 0x50uy ]; Description = "push rax"}
    let popRax = {Bytes = [ 0x58uy ]; Description = "pop rax"}
    let popR10 = {Bytes = [ 0x41uy; 0x5auy ] ; Description = "pop r10" }
    let pushI4 v = {Bytes = [0x6auy; byte(v)]; Description = sprintf "push %d" v}
    let cmpRaxR10 = {Bytes = [ 0x4cuy; 0x39uy ; 0xd0uy  ]; Description = "cmp rax, r10" }
    