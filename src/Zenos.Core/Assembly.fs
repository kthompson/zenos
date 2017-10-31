namespace Zenos.Framework

open System
open System.Reflection.Emit

module Assembly =
    open Zenos.Core
    open FParsec
    open System.IO

    // 64bit samples of nasm syntax https://www.csee.umbc.edu/portal/help/nasm/sample_64.shtml

    type Label = Label of Label:string

    type SectionEntry =
    | Label of Label
    | SectionEntry of Label option * Instruction

    type Directive =
    | Global of Name:string
    | Extern of Name:string
    | Section of Name:string * SectionEntry list

    type Listing = Listing of Directive list

    let (<!>) (p: Parser<_,_>) label : Parser<_,_> =
        fun stream ->
            printfn "%A: Entering %s" stream.Position label
            let reply = p stream
            printfn "%A: Leaving %s (%A)" stream.Position label reply.Status
            reply

    let wsChar = skipAnyOf "\t "
    let ws1 : Parser<unit, unit> =
        many1 wsChar
        |>> ignore 
        <?> "ws1" 
        <!> "ws1"

    let ws : Parser<unit, unit> =
        many wsChar 
        |>> ignore 
        <?> "ws"
        <!> "ws"

    let identifier label : Parser<string, unit> = 
        let isIdentifierFirstChar c = isLetter c || c = '_'
        let isIdentifierChar c = isLetter c || isDigit c || c = '_'

        many1Satisfy2L isIdentifierFirstChar isIdentifierChar "identifier"
        <?> label
        <!> label

    let stringLiteral quote : Parser<string, unit> =
        let escape =  anyOf "\"\\/bfnrt"
                      |>> function
                          | 'b' -> "\b"
                          | 'f' -> "\u000C"
                          | 'n' -> "\n"
                          | 'r' -> "\r"
                          | 't' -> "\t"
                          | c   -> string c // every other char is mapped to itself

        let unicodeEscape =
            /// converts a hex char ([0-9a-fA-F]) to its integer number (0-15)
            let hex2int c = (int c &&& 15) + (int c >>> 6)*9

            pstring "u" >>. pipe4 hex hex hex hex (fun h3 h2 h1 h0 ->
                (hex2int h3)*4096 + (hex2int h2)*256 + (hex2int h1)*16 + hex2int h0
                |> char |> string
            )

        let escapedCharSnippet = pstring "\\" >>. (escape <|> unicodeEscape)
        let normalCharSnippet  = manySatisfy (fun c -> c <> quote && c <> '\\')

        between (pchar quote) (pchar quote)
                (stringsSepBy normalCharSnippet escapedCharSnippet)

    let comment: Parser<unit, unit> =
        let firstChar = satisfy (fun c -> c = ';')
        let restChar = satisfy (fun c -> c <> '\n')

        firstChar >>. many restChar
        |>> ignore
        <?> "comment"
        <!> "comment"

    let restOfLine: Parser<unit, unit> = 
        ws .>> opt comment

    let pextern: Parser<Directive, unit> =
        let externName = identifier "extern name"

        pstring "extern" >>. ws1 >>. externName .>> restOfLine
        |>> Extern
        <?> "Extern"
        <!> "Extern"

    let pglobal: Parser<Directive, unit> =
        let globalName = identifier "global name"

        pstring "global" >>. ws1 >>. globalName .>> restOfLine
        |>> Global
        <?> "Global"
        <!> "Global"

    let preg8: Parser<Register8, unit> = 
        choice [
            pstring "ah" >>% AH
            pstring "bh" >>% BH
            pstring "ch" >>% CH
            pstring "dh" >>% DH
            pstring "al" >>% AL
            pstring "bl" >>% BL
            pstring "cl" >>% CL
            pstring "dl" >>% DL
            pstring "sil" >>% SIL
            pstring "dil" >>% DIL
            pstring "bpl" >>% BPL
            pstring "spl" >>% SPL
            pstring "r8b" >>% R8B
            pstring "r9b" >>% R9B
            pstring "r10b" >>% R10B
            pstring "r11b" >>% R11B
            pstring "r12b" >>% R12B
            pstring "r13b" >>% R13B
            pstring "r14b" >>% R14B
            pstring "r15b" >>% R15B
        ]
        <?> "8 Bit Register"

    let preg16: Parser<Register16, unit> = 
        choice [
            pstring "ax" >>% AX
            pstring "bx" >>% BX
            pstring "cx" >>% CX
            pstring "dx" >>% DX
            pstring "si" >>% SI
            pstring "di" >>% DI
            pstring "bp" >>% BP
            pstring "sp" >>% SP
            pstring "r8w" >>% R8W
            pstring "r9w" >>% R9W
            pstring "r10w" >>% R10W
            pstring "r11w" >>% R11W
            pstring "r12w" >>% R12W
            pstring "r13w" >>% R13W
            pstring "r14w" >>% R14W
            pstring "r15w" >>% R15W
        ]
        <?> "16 Bit Register"

    let preg32: Parser<Register32,unit> = 
        choice [
            pstring "eax" >>% EAX
            pstring "ebx" >>% EBX
            pstring "ecx" >>% ECX
            pstring "edx" >>% EDX
            pstring "esi" >>% ESI
            pstring "edi" >>% EDI
            pstring "ebp" >>% EBP
            pstring "esp" >>% ESP
            pstring "r8d" >>% R8D
            pstring "r9d" >>% R9D
            pstring "r10d" >>% R10D
            pstring "r11d" >>% R11D
            pstring "r12d" >>% R12D
            pstring "r13d" >>% R13D
            pstring "r14d" >>% R14D
            pstring "r15d" >>% R15D
            pstring "eip" >>% EIP
            pstring "flags" >>% FLAGS
        ]
        <?> "32 Bit Register"

    let preg64: Parser<Register64,unit> = 
        choice [
            pstring "rax" >>% RAX
            pstring "rbx" >>% RBX
            pstring "rcx" >>% RCX
            pstring "rdx" >>% RDX
            pstring "rsi" >>% RSI
            pstring "rdi" >>% RDI
            pstring "rbp" >>% RBP
            pstring "rsp" >>% RSP
            pstring "r8" >>% R8
            pstring "r9" >>% R9
            pstring "r10" >>% R10
            pstring "r11" >>% R11
            pstring "r12" >>% R12
            pstring "r13" >>% R13
            pstring "r14" >>% R14
            pstring "r15" >>% R15
            pstring "rip" >>% RIP
            pstring "rflags" >>% RFLAGS
        ]
        <?> "64 Bit Register"

    let preg: Parser<Register, unit> = 
        let p64 = preg64 |>> Register64
        let p32 = preg32 |>> Register32
        let p16 = preg16 |>> Register16
        let p8 = preg8 |>> Register8

        choice [
            p64
            p32
            p16
            p8
        ]

    let poperand: Parser<Operand, unit> =
        choice [
            preg |>> Register
            pint16 |>> Operand.Int16
            pint32 |>> Operand.Int32
            pint64 |>> Operand.Int64
            identifier "operand" |>> LabelReference
        ]

    let pdatainst: Parser<Instruction, unit> = 
        let ds =
            stringLiteral '\'' <|> stringLiteral '"'
            |>> DataString
            <!> "ds"
        let df = pfloat |>> DataFloat <!> "df"
        let di = pint64 |>> DataInt <!> "di"

        let dataItem =
            (ds <|> di <|> df) .>> ws

        let sep = pchar ',' >>. ws
        let dataList =
            sepBy1 dataItem sep
            <!> "datalist"

        let dtype =
            choice [
                pstring "db" >>% Data8
                pstring "dw" >>% Data16
                pstring "dd" >>% Data32
                pstring "dq" >>% Data64
            ]
            <!> "dtype"

        pipe2 dtype (ws1 >>. dataList)
            (fun a b -> (a b) |> Instruction.noOperands)
        <!> "pdatainst"

    let pInstruction : Parser<Instruction, unit> =
        let op = poperand
        let zeroOps ins code =
            pstring ins
            >>% (code |> X86_64 |> Instruction.noOperands)

        let oneOp ins code =
            pstring ins .>> ws1 >>. op
            |>> fun operand -> 
                let code = X86_64 code
                Instruction.oneOperand code operand

        let twoOp ins code =
            pstring ins .>> ws1 >>. op .>> ws .>> pchar ',' .>> ws .>>. op
            |>> fun (op1, op2) ->
                let code = X86_64 code
                Instruction.twoOperand code op1 op2

        choice [
            zeroOps "syscall" Syscall
            twoOp "add" Add
            twoOp "mov" Move
            twoOp "xor" Xor
            oneOp "pop" Pop
            oneOp "push" Push
            pdatainst
        ]
        <?> "Instruction"
        <!> "Instruction"
    
    let pLabel: Parser<Label, unit> =
        let label = identifier "label"

        label .>> pchar ':'
        |>> Label.Label
        <?> "Label"
        <!> "Label"
        
    let emptyLine: Parser<unit, unit> =
        choice [
            comment
            ws1 .>> opt comment
            nextCharSatisfies (fun c -> c = '\n')
        ]

    let pSectionEntry : Parser<SectionEntry option, unit> =
        let listing =
            let labelledInst = 
                pLabel .>> ws .>>. opt pInstruction
                |>> function
                    | label, Some(inst) -> SectionEntry (Some(label), inst)
                    | label, None -> SectionEntry.Label label
                <?> "LabelledInst"
                <!> "LabelledInst"

            let unlabelledInst =
                pInstruction
                |>> fun inst -> SectionEntry (None, inst)
                <?> "UnlabelledInst"
                <!> "UnlabelledInst"
        
            choice [
                (attempt labelledInst) |>> Some
                unlabelledInst |>> Some
                preturn None
            ]

        ws >>. listing .>> restOfLine
        <?> "SectionEntry"
        <!> "SectionEntry"

    let pSectionEntries: Parser<SectionEntry list, unit> =
        (sepBy pSectionEntry skipNewline)
        |>> fun items -> List.choose id items

    let pSection: Parser<Directive option, unit> =
        let sectionName = 
            let first = pchar '.' 
            let alpha = satisfy (fun c -> Char.IsLetterOrDigit(c) || c = '_' || c = '@') <?> "section name"

            first .>>. manyChars alpha
            |>> fun (a, b) -> a.ToString() + b
            <?> "section name"

        let sectionHeader = 
            pstring "section" <|> pstring "SECTION"

        let sectionLine = 
            (sectionHeader >>. ws1 >>. sectionName .>> restOfLine)
            <|> preturn ".text"
            <!> "sectionLine"

        sectionLine .>>. pSectionEntries
        |>> fun (name, items) ->
                if items.IsEmpty then None
                else Section (name, items) |> Some
        <?> "Section"
        <!> "Section"

    let pDirective : Parser<Directive option, unit> =
        let listing =
            choice [
                emptyLine >>% None
                pglobal |>> Some
                pextern |>> Some
                pSection
            ]

        
        ws >>. listing
        <?> "ListingEntry"
        <!> "ListingEntry"
    
    let pListing : Parser<Listing, unit> =
        (sepBy pDirective skipNewline) .>> eof
        |>> fun items ->
                items
                |> List.choose id
                |> Listing
        <?> "Listing"
        <!> "Listing"
    