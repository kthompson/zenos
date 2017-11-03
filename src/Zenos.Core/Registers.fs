namespace Zenos.Framework

open Mono.Cecil
open Mono.Cecil.Cil

type MonoInstruction = Mono.Cecil.Cil.Instruction

type Register64 =
| RAX | RBX | RCX | RDX 
| RSI | RDI | RBP | RSP 
| R8  | R9  | R10 | R11  
| R12 | R13 | R14 | R15  
| RIP | RFLAGS

type Register32 =
| EAX  | EBX  | ECX  | EDX  
| ESI  | EDI  | EBP  | ESP
| R8D  | R9D  | R10D | R11D 
| R12D | R13D | R14D | R15D
| EIP | FLAGS

type Register16 =
| AX   | BX   | CX   | SI
| DI   | DX   | BP   | SP
| R8W  | R9W  | R10W | R11W
| R12W | R13W | R14W | R15W

type Register8  = 
| AL   | BL   | CL   | DL
| AH   | BH   | CH   | DH
| SIL  | DIL  | BPL  | SPL
| R8B  | R9B  | R10B | R11B
| R12B | R13B | R14B | R15B

type Register = 
| Register64 of Register64
| Register32 of Register32
| Register16 of Register16
| Register8 of Register8



module Register64 =
    let create name =
        match name with
        | "rax" -> RAX |> Ok
        | "rbx" -> RBX |> Ok
        | "rcx" -> RCX |> Ok
        | "rdx" -> RDX |> Ok
        | "rsi" -> RSI |> Ok
        | "rdi" -> RDI |> Ok
        | "rbp" -> RBP |> Ok
        | "rsp" -> RSP |> Ok
        | "r8" -> R8 |> Ok
        | "r9" -> R9 |> Ok
        | "r10" -> R10 |> Ok
        | "r11" -> R11 |> Ok
        | "r12" -> R12 |> Ok
        | "r13" -> R13 |> Ok
        | "r14" -> R14 |> Ok
        | "r15" -> R15 |> Ok
        | "rip" -> RIP |> Ok
        | "rflags" -> RFLAGS |> Ok
        | s -> sprintf "unknown register %s" s |> Error 

module Register32 =
    let create name =
        match name with
        | "eax" -> EAX |> Ok
        | "ebx" -> EBX |> Ok
        | "ecx" -> ECX |> Ok
        | "edx" -> EDX |> Ok
        | "esi" -> ESI |> Ok
        | "edi" -> EDI |> Ok
        | "ebp" -> EBP |> Ok
        | "esp" -> ESP |> Ok
        | "r8d" -> R8D |> Ok
        | "r9d" -> R9D |> Ok
        | "r10d" -> R10D |> Ok
        | "r11d" -> R11D |> Ok
        | "r12d" -> R12D |> Ok
        | "r13d" -> R13D |> Ok
        | "r14d" -> R14D |> Ok
        | "r15d" -> R15D |> Ok
        | "eip" -> EIP |> Ok
        | "flags" -> FLAGS |> Ok
        | s -> sprintf "unknown register %s" s |> Error 

module Register =
    open Zenos.Core.Result
    let create name =
        let r64 =
            Register64.create name
            |> Result.map Register64

        let r32 =
            Register32.create name
            |> Result.map Register32

        let (<|>) = orElse
        r64 <|> r32
