namespace Zenos.Framework


open Mono.Cecil
open Mono.Cecil.Cil

open Zenos.Core


type CilOffset = int

//type ZenosOpCode = 
//| Load
//| Store
//| Local
//| Argument
//| PushConstant
//| PushArgument
//| Pop

type Operand =
| ImmediateByte of byte
| ImmediateSByte of sbyte
| ImmediateInt16 of int16
| ImmediateInt32 of int32
| ImmediateSingle of float32
| ImmediateDouble of float
| ImmediateInt64 of int64
| ImmediateString of string

| LabelReference of string
| Register of Register

| InstructionOperand of Instruction
| InstructionsOperand of Instruction list

// TODO: convert these to non-cecil types ?
| TypeReferenceOperand of TypeReference
| MethodReferenceOperand of MethodReference
| FieldReferenceOperand of FieldReference
| VariableOperand of VariableDefinition
| ParameterOperand of ParameterDefinition
| CallSiteOperand of CallSite


and X86_64 =
| Add of src:Operand * dest:Operand
| Move of src:Operand * dest:Operand
| Pop of src:Operand
| Push of src:Operand
| Syscall
| Xor of src:Operand * dest:Operand

and Data =
| DataString of string
| DataFloat of float
| DataInt of int64

and Instruction = 
//| Zenos of OpCode:ZenosOpCode
| Cil of OpCode:Code * CilOffset * Operand option
| X86_64 of OpCode:X86_64
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
        | :? string as typ -> typ |> ImmediateString |> Some |> Ok
        | :? sbyte as typ -> typ |> ImmediateSByte |> Some |> Ok
        | :? byte as typ -> typ |> ImmediateByte |> Some |> Ok
        | :? int as typ -> typ |> ImmediateInt32 |> Some |> Ok
        | :? int64 as i -> i |> ImmediateInt64 |> Some |> Ok
        | :? float32 as f -> f |> ImmediateSingle |> Some |> Ok
        | :? float as f -> f |> ImmediateDouble |> Some |> Ok
        | :? VariableDefinition as v -> v |> VariableOperand |> Some |> Ok
        | :? ParameterDefinition as parameter -> parameter |> ParameterOperand |> Some |> Ok
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
   