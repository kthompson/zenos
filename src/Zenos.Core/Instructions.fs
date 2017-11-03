namespace Zenos.Framework


open Mono.Cecil
open Mono.Cecil.Cil


type Operand =
| ImmediateSByte of sbyte
| ImmediateInt16 of int16
| ImmediateInt32 of int32
| ImmediateInt64 of int64

| LabelReference of string

| Register of Register
//| Instruction of Instruction

type ZenosOpCode = 
| Load
| Store
| Local
| Argument
| PushConstant
| PushArgument
| Pop

type X86_64 =
| Add of src:Operand * dest:Operand
| Move of src:Operand * dest:Operand
| Pop of src:Operand
| Push of src:Operand
| Syscall
| Xor of src:Operand * dest:Operand

type Data =
| DataString of string
| DataFloat of float
| DataInt of int64

type Instruction = 
| Zenos of OpCode:ZenosOpCode
| Cil of OpCode:Code
| X86_64 of OpCode:X86_64
| Data8 of Data list
| Data16 of Data list
| Data32 of Data list
| Data64 of Data list

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
   