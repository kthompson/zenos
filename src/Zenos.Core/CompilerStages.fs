namespace Zenos.Stages

open Zenos.Core
open Zenos.Framework

module CilSimplifier =
    type Simplifier = MethodContext * CilInstruction -> CilInstruction
    type Simplifiers = Map<CilCode, Simplifier>

    let addSimplifier (simplifier: Simplifier) (codes: CilCode list) (simplifiers: Simplifiers) : Simplifiers =
        codes
        |> List.fold(fun map code ->
            map.Add(code , simplifier)
        ) simplifiers

    
    let simplifyFromSByte (inst: CilInstruction) op =
        {inst with Code = op}

    //let addSByteSimplifier sByteIns dest =
        //addSimplifier()

    //let instSimplifier: Compiler<Instruction, Instruction list> =
    //    let compiler inst =
    //        match inst with
    //        | Cil {Code = code; Offset = offset; Operand = None } ->
    //            match code with
    //            | CilCode.Nop -> X86Code.Nop |> X86_64
    //            | CilCode.Ldc_I4_1 -> 1L |> ImmediateInt64 |> Immediate |> Push |> X86_64
    //            | _ -> inst
    //        | Cil {Code = code; Offset = offset; Operand = Some operand } -> inst
    //        | _ -> inst

    //    Compiler compiler

module CompilerStages =
    open System

    let cecilToX86: Compiler<MethodContext> =
        let instCompiler : Compiler<Instruction, Instruction list> =
            // common ops
            let pushRax = Emit ([ 0x50uy ], "push rax")
            let popRax = Emit ([ 0x58uy ], "pop rax")
            let compiler inst =
                match inst with
                | Cil {Code = code; Offset = offset; Operand = None } ->
                    match code with
                    | CilCode.Add ->
                        [
                            Emit ([ 0x41uy; 0x5auy ], "pop r10")
                            popRax
                            Emit ([ 0x4cuy; 0x01uy ; 0xd0uy  ], "add rax, r10")
                            pushRax                        
                        ]
                    | CilCode.Nop -> [ ]     
                    //| CilCode.Ldarg_0 ->
                    //    [
                    //        I think this would be used for the `this` arg  
                    //        Emit ([ 0x51uy ], "push rcx")
                    //    ]
                    | CilCode.Ldarg_1 ->
                        [
                            Emit ([ 0x51uy ], "push rcx")
                        ]
                    | CilCode.Ldarg_2 ->
                        [
                            Emit ([ 0x52uy ], "push rdx")
                        ]
                    | CilCode.Ldarg_3 ->
                        [
                            Emit ([ 0x41uy; 0x50uy ], "push r8")
                        ]
                    | CilCode.Ldc_I4_M1 ->
                        [ 
                            Emit ([0x6auy; 0xffuy], "push -1")
                        ]            
                    | CilCode.Ldc_I4_0 ->
                        [ 
                            Emit ([0x6auy; 0x00uy], "push 0")
                        ]
                    | CilCode.Ldc_I4_1 ->
                        [ 
                            Emit ([0x6auy; 0x01uy], "push 1")
                        ]
                    | CilCode.Stloc_0 ->
                        [
                            popRax
                            Emit ([ 0x48uy; 0x89uy; 0x45uy; 0xf8uy ], "mov QWORD PTR [rbp-0x8],rax")
                        ]
                    | CilCode.Ldloc_0 ->
                        [
                            Emit ([ 0x48uy; 0x8buy; 0x45uy; 0xf8uy ], "mov rax, QWORD PTR [rbp-0x8]")
                            pushRax
                        ]

                    | CilCode.Ret ->
                        [
                            // TODO: depends on if there is a return variable or not
                            popRax
                        ]
                    | _ -> 
                        failwithf "Unsupported instruction: %A" inst       
                | Cil {Code = code; Offset = offset; Operand = Some (ParameterOperand p) } ->
                    match code with
                    | CilCode.Ldarg_S ->
                        match p with
                        | 4 ->
                            [
                                Emit ([ 0x41uy; 0x51uy ], "push r9")
                            ]
                        | i when i >= 5 && i < 17 ->

                            let offset = byte((p + 2) * 8)
                            [
                                Emit ([0x48uy; 0x8buy; 0x45uy; offset], sprintf "mov rax,QWORD PTR [rbp+0x%X]" offset)
                                pushRax
                            ]
                        | _ -> failwithf "Unsupported parameter %d index for instruction: %A" p inst    
                    | _ -> 
                        failwithf "Unsupported instruction: %A" inst    
                | Cil {Code = CilCode.Br_S; Offset = offset; Operand = Some (InstructionOperand (Cil ins)) } ->
                    if ins.Offset = offset + 2 then
                        []
                    else
                        failwithf "Unsupported instruction: %A" inst
                | Cil {Code = code; Offset = offset; Operand = Some (Immediate (ImmediateSByte sb)) } ->
                    match code with
                    | CilCode.Ldc_I4_S ->
                        [ 
                            Emit ([0x6auy; byte(sb)], sprintf "push %d" sb)
                        ]
                    | CilCode.Ldarga_S ->
                        match sb with
                        | 4y ->
                            [
                                Emit ([ 0x41uy; 0x51uy ], "push r9")
                            ] 
                        | _ -> failwithf "Unsupported instruction: %A" inst    
                    | _ -> 
                        failwithf "Unsupported instruction: %A" inst    
                | Cil {Code = code; Offset = offset; Operand = Some (Immediate (ImmediateInt32 i32)) } ->
                    match code with
                    | CilCode.Ldc_I4 ->
                        let i32Bytes = BitConverter.GetBytes(i32) |> List.ofArray
                        [ 
                            Emit (0x68uy :: i32Bytes, sprintf "push %d" i32)
                        ]
                    | _ -> 
                        failwithf "Unsupported instruction: %A" inst    
                | Cil {Code = code; Offset = offset; Operand = Some (Immediate (ImmediateInt64 i64)) } ->
                    match code with
                    | CilCode.Ldc_I8 ->
                        let i64Bytes = BitConverter.GetBytes(i64) |> List.ofArray
                        [ 
                            Emit (0x48uy :: 0xb8uy :: i64Bytes, sprintf "mov rax, %d" i64)
                            Emit ([ 0x50uy ], "push rax")
                        ]
                    | _ -> 
                        failwithf "Unsupported instruction: %A" inst    
                | Cil {Code = code; Offset = offset; Operand = Some operand } -> [ inst ]
                | _ ->
                    failwithf "Unsupported instruction: %A" inst

            Compiler compiler

        let compiler method =
            let newInstructions = 
                method.Instructions
                |> List.collect(Compiler.run instCompiler)

            let prologue =
                [
                    Emit ([ 0x55uy ], "push rbp")
                ]

            let epilogue =
                [
                    Emit ([ 0x5duy ], "pop rbp")
                    Emit ([ 0xc3uy ], "ret")
                ]
            
            {method with Instructions = List.concat [ prologue; newInstructions; epilogue ] }

        Compiler compiler

    let emit: Compiler<MethodContext> =
        let compiler method = 
            let code =
                method.Instructions
                |> List.collect(function
                | Emit (data, _) -> data 
                | ins ->
                    failwithf "unsupported instruction %A" ins
                )
                |> Array.ofList

            {method with Code = code}
        Compiler compiler

    let branchOptimization : Compiler<MethodContext> = Compiler.empty
    let deadCodeElimination : Compiler<MethodContext> = Compiler.empty
    let localPropagation : Compiler<MethodContext> = Compiler.empty
    let staticSingleAssignmentTranslation : Compiler<MethodContext> = Compiler.empty
    let printer writer : Compiler<MethodContext> =
        let compiler (context: MethodContext) =
            context.Instructions
            |> List.iter(function
                | Emit (_, desc) -> writer desc
                | inst -> sprintf "%A" inst |> writer
                
            )
            context
        Compiler compiler

