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
            let popR10 = Emit ([ 0x41uy; 0x5auy ], "pop r10")
            let pushI4 v = 
                [ 
                    Emit ([0x6auy; byte(v)], sprintf "push %d" v)
                ]
            let unaryOp op = [ popRax; op; pushRax ]
            let binaryOp op = popR10 :: unaryOp op
            let cmp op =
                [
                
                            popR10
                            popRax
                            Emit ([ 0x4cuy; 0x39uy ; 0xd0uy  ],         "cmp rax, r10")
                            op
                            Emit ([  0x24uy; 0x01uy],                   "and    al,0x1")
                            Emit ([  0x48uy; 0x0fuy; 0xb6uy; 0xc0uy],   "movzx  rax,al")
                            pushRax
                ]

            let storeLocal index = 
                let offset = ((index + 1) * 8)
                let b = byte(0x100 - offset)
                [
                    popRax
                    Emit ([ 0x48uy; 0x89uy; 0x45uy; b ], sprintf "mov QWORD PTR [rbp-0x%x], rax" offset)
                ]
            let loadLocal index = 
                let offset = ((index + 1) * 8)
                let b = byte(0x100 - offset)
                [
                    Emit ([ 0x48uy; 0x8buy; 0x45uy; b ], sprintf "mov rax, QWORD PTR [rbp-0x%x]" offset)
                    pushRax
                ]
            let compiler inst =
                match inst with
                | Cil {Code = code; Offset = offset; Operand = None } ->
                    match code with
                    | CilCode.And ->
                        ([ 0x4cuy; 0x21uy ; 0xd0uy  ], "and rax, r10")
                        |> Emit
                        |> binaryOp
                    | CilCode.Add ->
                        ([ 0x4cuy; 0x01uy ; 0xd0uy  ], "add rax, r10")
                        |> Emit
                        |> binaryOp
                    | CilCode.Ceq ->
                        ([  0x0fuy; 0x94uy; 0xc0uy ],          "sete   al")
                        |> Emit
                        |> cmp
                    | CilCode.Cgt ->
                        ([  0x0fuy; 0x9fuy; 0xc0uy ],          "setg   al")
                        |> Emit
                        |> cmp
                    | CilCode.Clt ->
                        ([  0x0fuy; 0x9cuy; 0xc0uy ],          "setl   al")
                        |> Emit
                        |> cmp
                    | CilCode.Conv_I8 ->
                        ([0x48uy; 0x63uy; 0xc0uy ], "movsxd      rax,eax")
                        |> Emit
                        |> unaryOp
                        
                    | CilCode.Div ->
                         [
                            popR10
                            popRax
                            Emit ([0x48uy; 0x99uy], "cqo")
                            Emit ([ 0x49uy; 0xf7uy ; 0xfauy ], "idiv r10")
                            pushRax
                        ]
                        
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
                    | CilCode.Ldc_I4_0 -> pushI4 0
                    | CilCode.Ldc_I4_1 -> pushI4 1
                    | CilCode.Ldc_I4_2 -> pushI4 2
                    | CilCode.Ldc_I4_3 -> pushI4 3
                    | CilCode.Ldc_I4_4 -> pushI4 4
                    | CilCode.Ldc_I4_5 -> pushI4 5
                    | CilCode.Ldc_I4_6 -> pushI4 6
                    | CilCode.Ldc_I4_7 -> pushI4 7
                    | CilCode.Ldc_I4_8 -> pushI4 8
                    | CilCode.Ldloc_0 -> loadLocal 0
                    | CilCode.Ldloc_1 -> loadLocal 1
                    | CilCode.Ldloc_2 -> loadLocal 2
                    | CilCode.Ldloc_3 -> loadLocal 3
                    | CilCode.Mul ->
                        ([ 0x49uy; 0x0fuy ; 0xafuy; 0xc2uy ], "imul rax, r10")
                        |> Emit
                        |> binaryOp
                    | CilCode.Neg ->
                        ([0x48uy; 0xf7uy; 0xd8uy], "neg rax")
                        |> Emit 
                        |> unaryOp
                    | CilCode.Nop -> [ ]  
                    | CilCode.Not ->
                        ([0x48uy; 0xf7uy; 0xd0uy], "not rax")
                        |> Emit 
                        |> unaryOp
                    | CilCode.Or ->
                        ([ 0x4cuy; 0x09uy ; 0xd0uy  ], "or rax, r10")
                        |> Emit
                        |> binaryOp
                    | CilCode.Stloc_0 -> storeLocal 0
                    | CilCode.Stloc_1 -> storeLocal 1
                    | CilCode.Stloc_2 -> storeLocal 2
                    | CilCode.Stloc_3 -> storeLocal 3
                    | CilCode.Sub ->
                        ([ 0x4cuy; 0x29uy ; 0xd0uy  ], "sub rax, r10")
                        |> Emit
                        |> binaryOp

                    | CilCode.Ret ->
                        [
                            // TODO: depends on if there is a return variable or not
                            popRax
                        ]
                    | CilCode.Xor ->
                        ([ 0x4cuy; 0x31uy ; 0xd0uy  ], "xor rax, r10")
                        |> Emit
                        |> binaryOp
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

