namespace Zenos.Stages

open Zenos.Core
open Zenos.Framework

module CilSimplifier =
    type Simplifier = MethodContext * CilInstruction -> CilInstruction option
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
    //let simplifier: Compiler<MethodContext> =

module CompilerStages =
    open System
    open X86Instruction

    let cecilToX86: Compiler<MethodContext> =
        let instCompiler : Compiler<Instruction, Instruction> =
            // common ops

            let unaryOp op = [ popRax; op; pushRax ]
            let binaryOp op = popR10 :: unaryOp op
            let cmp op =
                [
                    popR10
                    popRax
                    {Bytes = [ 0x4cuy; 0x39uy ; 0xd0uy  ]; Description = "cmp rax, r10" }
                    op
                    {Bytes = [ 0x24uy; 0x01uy  ]; Description = "and    al,0x1" }
                    {Bytes = [ 0x48uy; 0x0fuy; 0xb6uy; 0xc0uy  ]; Description = "movzx  rax,al" }
                    pushRax
                ]

            let storeLocal index = 
                let offset = ((index + 1) * 8)
                let b = byte(0x100 - offset)
                [
                    popRax
                    {Bytes = [ 0x48uy; 0x89uy; 0x45uy; b ]; Description = sprintf "mov QWORD PTR [rbp-0x%x], rax" offset }
                ]
            let loadLocal index = 
                let offset = ((index + 1) * 8)
                let b = byte(0x100 - offset)
                [
                    {Bytes = [ 0x48uy; 0x8buy; 0x45uy; b ]; Description = sprintf "mov rax, QWORD PTR [rbp-0x%x]" offset }
                    pushRax
                ]
            let compiler inst =
                match inst with
                | Cil {Code = code; Offset = offset } ->
                
                    let mkInst list =
                        Emit (Some offset, list)

                    match code with
                    | And ->
                        {Bytes = [ 0x4cuy; 0x21uy ; 0xd0uy  ]; Description = "and rax, r10" }
                        |> binaryOp
                        |> mkInst
                    | CilCode.Add ->
                        {Bytes = [ 0x4cuy; 0x01uy ; 0xd0uy  ]; Description = "add rax, r10" }
                        |> binaryOp
                        |> mkInst
                    | Ceq ->
                        {Bytes = [  0x0fuy; 0x94uy; 0xc0uy ]; Description = "sete   al" }
                        |> cmp
                        |> mkInst
                    | Cgt ->
                        {Bytes = [  0x0fuy; 0x9fuy; 0xc0uy ]; Description = "setg   al" }
                        |> cmp
                        |> mkInst
                    | Clt ->
                        {Bytes = [  0x0fuy; 0x9cuy; 0xc0uy ]; Description = "setl   al" }
                        |> cmp
                        |> mkInst
                    | Conv_I8 ->
                        {Bytes = [0x48uy; 0x63uy; 0xc0uy ]; Description = "movsxd rax,eax" }
                        |> unaryOp
                        |> mkInst
                    | Div ->
                         mkInst [
                            popR10
                            popRax
                            {Bytes = [0x48uy; 0x99uy]; Description = "cqo" }
                            {Bytes = [ 0x49uy; 0xf7uy ; 0xfauy ]; Description = "idiv r10" }
                            pushRax
                        ]
                        
                    | Ldarg_1 ->
                        [
                            {Bytes = [ 0x51uy ]; Description = "push rcx" }
                        ]
                        |> mkInst
                    | Ldarg_2 ->
                        [
                            {Bytes = [ 0x52uy ]; Description = "push rdx"}
                        ]
                        |> mkInst
                    | Ldarg_3 ->
                        [
                            {Bytes = [ 0x41uy; 0x50uy ]; Description = "push r8" }
                        ]
                        |> mkInst
                    | Ldc_I4_M1 ->
                        [ 
                            {Bytes = [0x6auy; 0xffuy]; Description = "push -1" }
                        ]        
                        |> mkInst    
                    | Ldc_I4_0 -> pushI4 0 :: List.empty |> mkInst
                    | Ldc_I4_1 -> pushI4 1 :: List.empty |> mkInst
                    | Ldc_I4_2 -> pushI4 2 :: List.empty |> mkInst
                    | Ldc_I4_3 -> pushI4 3 :: List.empty |> mkInst
                    | Ldc_I4_4 -> pushI4 4 :: List.empty |> mkInst
                    | Ldc_I4_5 -> pushI4 5 :: List.empty |> mkInst
                    | Ldc_I4_6 -> pushI4 6 :: List.empty |> mkInst
                    | Ldc_I4_7 -> pushI4 7 :: List.empty |> mkInst
                    | Ldc_I4_8 -> pushI4 8 :: List.empty |> mkInst
                    | Ldloc_0 -> loadLocal 0 |> mkInst
                    | Ldloc_1 -> loadLocal 1 |> mkInst
                    | Ldloc_2 -> loadLocal 2 |> mkInst
                    | Ldloc_3 -> loadLocal 3 |> mkInst
                    | Mul ->
                        {Bytes = [ 0x49uy; 0x0fuy ; 0xafuy; 0xc2uy ]; Description = "imul rax, r10" }
                        |> binaryOp
                        |> mkInst
                    | Neg ->
                        {Bytes = [0x48uy; 0xf7uy; 0xd8uy]; Description = "neg rax" }
                        |> unaryOp
                        |> mkInst
                    | CilCode.Nop -> mkInst [ ]  
                    | Not ->
                        {Bytes = [0x48uy; 0xf7uy; 0xd0uy]; Description = "not rax" }
                        |> unaryOp
                        |> mkInst
                    | Or ->
                        {Bytes = [ 0x4cuy; 0x09uy ; 0xd0uy  ]; Description = "or rax, r10" }
                        |> binaryOp
                        |> mkInst
                    | Stloc_0 -> storeLocal 0 |> mkInst
                    | Stloc_1 -> storeLocal 1 |> mkInst
                    | Stloc_2 -> storeLocal 2 |> mkInst
                    | Stloc_3 -> storeLocal 3 |> mkInst
                    | Sub ->
                        {Bytes = [ 0x4cuy; 0x29uy ; 0xd0uy  ]; Description = "sub rax, r10" }
                        |> binaryOp
                        |> mkInst

                    | Ret ->
                        [
                            // TODO: depends on if there is a return variable or not
                            popRax
                        ]
                        |> mkInst
                    | CilCode.Xor ->
                        {Bytes = [ 0x4cuy; 0x31uy ; 0xd0uy  ]; Description = "xor rax, r10" }
                        |> binaryOp
                        |> mkInst
                    | Ldarg_S p ->
                        match p with
                        | 4y ->
                            [
                                {Bytes = [ 0x41uy; 0x51uy ]; Description = "push r9" }
                            ]
                            |> mkInst
                        | i when i >= 5y && i < 17y ->
                            let offset = byte((p + 2y) * 8y)
                            [
                                {Bytes = [0x48uy; 0x8buy; 0x45uy; offset]; Description = sprintf "mov rax,QWORD PTR [rbp+0x%X]" offset }
                                pushRax
                            ]
                            |> mkInst
                        | _ -> failwithf "Unsupported parameter %d index for instruction: %A" p inst    
                    | Br_S targetOffset ->
                        if targetOffset = offset + 2 then
                            [] |> mkInst
                        else
                            failwithf "Unsupported instruction: %A" inst
                    | Ldc_I4_S sb ->
                        [ 
                            {Bytes = [0x6auy; byte(sb)]; Description = sprintf "push %d" sb }
                        ]
                        |> mkInst
                    | Ldc_I4 i32 ->
                        let i32Bytes = BitConverter.GetBytes(i32) |> List.ofArray
                        [ 
                            {Bytes = 0x68uy :: i32Bytes; Description = sprintf "push %d" i32 }
                        ]
                        |> mkInst
                    | Ldc_I8 i64 ->
                        let i64Bytes = BitConverter.GetBytes(i64) |> List.ofArray
                        [ 
                            {Bytes = 0x48uy :: 0xb8uy :: i64Bytes; Description = sprintf "mov rax, %d" i64 }
                            {Bytes = [ 0x50uy ]; Description = "push rax" }
                        ]
                        |> mkInst
                    | _ -> 
                        failwithf "Unsupported instruction: %A" inst    
                | _ ->
                    failwithf "Unsupported instruction: %A" inst

            Compiler compiler

        let compiler method =
            let newInstructions = 
                method.Instructions
                |> List.map(Compiler.run instCompiler)

            let prologue =
                let bytes = [
                    {Bytes = [ 0x55uy ]; Description = "push rbp" }
                ]

                Emit (None, bytes) 

            let epilogue =
                let bytes = [
                    {Bytes = [ 0x5duy ]; Description = "pop rbp" }
                    {Bytes = [ 0xc3uy ]; Description = "ret" }
                ]

                Emit (None, bytes) 
            
            {method with Instructions = List.concat [ [prologue]; newInstructions; [epilogue] ] }

        Compiler compiler

    let emit: Compiler<MethodContext> =
        let compiler method = 
            let code =
                method.Instructions
                |> List.collect(function
                | Emit (_, data) ->
                    data
                    |> List.collect(fun x -> x.Bytes)
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
                | Emit (offset, instrs) ->
                    match offset with
                    | Some offset ->
                        sprintf "%X:" (int(offset)) |> writer
                    | None -> ()

                    instrs
                    |> List.iter(fun inst ->
                        sprintf "    %s" inst.Description |> writer 
                    )
                | inst -> sprintf "%A" inst |> writer
                
            )
            context
        Compiler compiler

