namespace Zenos.Framework

open Mono.Cecil

type CecilInst = Cil.Instruction
type ZenosInst = Instruction

type MethodContext = {Code: byte list; Instructions: ZenosInst list}
type TypeName = TypeName of string
type MethodName = MethodName of string
type TypeContext = {Methods: Map<MethodName, MethodContext>}
type AssemblyContext = {Types: Map<TypeName, TypeContext>}
type Compiler<'a> = Compiler of ('a -> 'a)

module MethodContext =
    let FromMethodDefinition (methodDefinition: MethodDefinition) = 
        let mkInstr (inst: CecilInst) : ZenosInst =
            match CecilOperand.Create inst.Operand with
            | Some operand ->
                ZenosInst.Cil (inst.OpCode.Code, Choice2Of2 operand)
            | None ->
                ZenosInst.Cil (inst.OpCode.Code, Choice1Of2 inst.Offset) 
                

        let instructions =
            methodDefinition.Body.Instructions
            |> Seq.map(mkInstr)
            |> List.ofSeq
        
        {Code = List.empty; Instructions = instructions}

module TypeContext =
    let Add name method tc =
        {tc with Methods = tc.Methods.Add(MethodName name, method)}

module AssemblyContext =
    let Add name typ tc =
        {tc with Types = tc.Types.Add(TypeName name, typ)}

module Compiler =
    let Empty = Compiler id
    let Run (Compiler compiler) context = compiler context

    let Create f = Compiler f

    let CreateStaged (list: 'a Compiler list) : 'a Compiler =
        list
        |> List.map(fun (Compiler f) -> f)
        |> List.fold(>>) id
        |> Compiler

