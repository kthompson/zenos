namespace Zenos.Framework

open Zenos.Core
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
    let FromMethodDefinition (methodDefinition: MethodDefinition) : Result<MethodContext, string list> = 
        let mkInstr (inst: CecilInst) : CilOffset * (ZenosInst * obj) =
            (inst.Offset, (ZenosInst.Cil (inst.OpCode.Code, inst.Offset, None), inst.Operand))

        let offsets = 
            methodDefinition.Body.Instructions
            |> Seq.map(fun ins -> ins.Offset)
            |> List.ofSeq

        let instOpMap =
            methodDefinition.Body.Instructions
            |> Seq.map(mkInstr)
            |> Map.ofSeq
            
        let instMap =
            instOpMap
            |> Map.map(fun _ (inst, _) -> inst)
        
        offsets
        |> List.map(fun offset ->
            offset
            |> instOpMap.TryFind 
            |> Result.ofOption [ (sprintf "Instruction not found as offset %d" offset) ]
            |> Result.bind(function
            | (ZenosInst.Cil (code, _, _), cecilOperand) ->
                CecilOperand.Create cecilOperand instMap
                |> Result.mapErrorToList
                |> Result.map(fun x ->
                    ZenosInst.Cil (code, offset, x)
                )
            | _ -> Error ["Unexpected instruction type"]
            )
        )
        |> Result.sequence
        |> Result.map(fun instructions ->
            {Code = List.empty; Instructions = instructions}
        )

        

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

