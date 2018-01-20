namespace Zenos.Framework

open Zenos.Core
open Mono.Cecil

type CecilInst = Cil.Instruction
type ZenosInst = Instruction

type MethodContext = {Code: byte array; Instructions: ZenosInst list}
type TypeName = TypeName of string
type MethodName = MethodName of string
type TypeContext = {Methods: Map<MethodName, MethodContext>}
type AssemblyContext = {Types: Map<TypeName, TypeContext>}
type Compiler<'a, 'b> = Compiler of ('a -> 'b)
type Compiler<'a> = Compiler<'a, 'a>

module MethodContext =
    let FromMethodDefinition (methodDefinition: MethodDefinition) : Result<MethodContext, string list> = 
        methodDefinition.Body.Instructions
        |> Seq.map(Instruction.fromCecil >> Result.mapErrorToList)
        |> List.ofSeq
        |> Result.sequence
        |> Result.map(fun instructions ->
            {Code = [| |]; Instructions = instructions}
        )


module TypeContext =
    let Add name method tc =
        {tc with Methods = tc.Methods.Add(MethodName name, method)}

module AssemblyContext =
    let Add name typ tc =
        {tc with Types = tc.Types.Add(TypeName name, typ)}

[<RequireQualifiedAccess>]
module Compiler =
    let empty = Compiler id
    let run (Compiler compiler) context = compiler context

    let create f = Compiler f

    let createStaged (list: 'a Compiler list) : 'a Compiler =
        list
        |> List.map(fun (Compiler f) -> f)
        |> List.fold(>>) id
        |> Compiler

    let createFromInstructionCompiler (inst : Compiler<Instruction, Instruction list>) : Compiler<MethodContext> =
        let compiler method =
            let newInstructions = 
                method.Instructions
                |> List.collect(run inst)

            {method with Instructions = newInstructions}

        Compiler compiler
