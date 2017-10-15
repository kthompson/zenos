namespace Zenos.Assembler.Tests

open Xunit
open Xunit.Abstractions

open System
open System.IO

open FParsec
open Zenos.Framework
open Zenos.Framework.Assembly

type AssemblyTests(output:ITestOutputHelper) =

    let file name = 
        File.ReadAllText(Path.Combine("resources", name))

    let zeroOpIns code =
        let code = X86_64 code
        let inst = 
            Instruction.noOperands code
            |> Instruction

        SectionEntry (None, inst)

    let oneOpIns code op1 =
        let code = X86_64 code
        let inst = 
            Instruction.oneOperand code op1
            |> Instruction

        SectionEntry (None, inst)

    let twoOpIns code op1 op2 =
        let code = X86_64 code
        let inst = 
            Instruction.twoOperand code op1 op2
            |> Instruction

        SectionEntry (None, inst)

    let popIns = oneOpIns Pop
    let pushIns = oneOpIns Push
    let moveIns = twoOpIns Move
    let xorIns = twoOpIns Xor

    do
        let tw = {
            new StringWriter() with 
                override x.Write(c: Char): Unit =
                    if c = '\n' then
                        let sb = x.GetStringBuilder()
                        output.WriteLine(sb.ToString())
                        sb.Clear() |> ignore
                    else base.Write(c)
                
        }

        System.Console.SetOut(tw)

    let test p str =
        printfn "hello"
        match run p str with
        | Success(result, _, _)   ->
            result
        | Failure(errorMsg, _, _) ->
            output.WriteLine(sprintf "Failure: %s" errorMsg)
            Assert.True false
            raise (Exception "")
            

    [<Fact>]
    let ``identifier "name" should parse "_cool"`` () =
        let p = identifier "name"

        let result = test p "_cool"

        Assert.Equal(result, "_cool")

    [<Fact>]
    let ``identifier "name" should not parse "'_cool"`` () =
        let p = identifier "name"

        match run p "'_cool" with
        | Success(result, _, _)   ->
            Assert.True false
            raise (Exception "")
        | Failure(errorMsg, _, _) ->
            output.WriteLine(sprintf "Failure: %s" errorMsg)

            Assert.Equal(errorMsg, "Error in Ln: 1 Col: 1\r\n'_cool\r\n^\r\nExpecting: name\r\n")
        
        

    [<Fact>]
    let ``plisting should parse empty input`` () =
        let actual = test plisting ""

        let expected = 
            []
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse comments-only.asm`` () =
        let ``comments-only.asm`` = file "comments-only.asm"

        let actual = test plisting ``comments-only.asm``

        let expected = 
            []
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse hello.asm`` () =
        let ``hello.asm`` = file "hello.asm"

        let actual = test plisting ``hello.asm``

        let expected = 
            [ 
                Global "_start"
                Section ".text"
                "_start" |> Label.Label  |> ListingEntry.Label
                
                moveIns (RAX |> Register64 |> Register) (Int16 1s)
                moveIns (RDI |> Register64 |> Register) (Int16 1s)
                moveIns (RSI |> Register64 |> Register) (Operand.Label "message")
                moveIns (RDX |> Register64 |> Register) (Int16 13s)
                zeroOpIns Syscall

                moveIns (EAX |> Register32 |> Register) (Int16 60s)
                xorIns (RDI |> Register64 |> Register) (RDI |> Register64 |> Register)                
                zeroOpIns Syscall
                "message" |> Label.Label  |> ListingEntry.Label
                // db      "Hello, World", 10      ; note the newline at the end
                SectionEntry (None, Data8 [ DataString "Hello, World"; DataInt 10L ])
            ]
            |> Listing
            
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse globals`` () =
        let actual = test plisting "global _start"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse global with whitespace`` () =
        let actual = test plisting "  global _start"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing  

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse global with whitespace and comment`` () =
        let actual = test plisting "  global _start; bleh"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)


    [<Fact>]
    let ``plisting should parse global with whitespace and comment2`` () =
        let actual = test plisting "  global _start ; bleh"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)


    [<Fact>]
    let ``plisting should parse global with comment2`` () =
        let actual = test plisting "global _start ; bleh"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)


    [<Fact>]
    let ``plisting should parse extern`` () =
        let actual = test plisting """extern _start"""

        let expected = 
            [ 
                Extern "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse extern with newLine`` () =
        let actual = test plisting """extern _start
"""

        let expected = 
            [ 
                Extern "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse extern twice`` () =
        let str = """extern first
extern second"""
        let actual = test plisting str

        let expected = 
            [ 
                Extern "first"
                Extern "second"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse global and extern`` () =
        let actual = test plisting """global _start
extern _start"""

        let expected = 
            [ 
                Global "_start"
                Extern "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse a label`` () =
        let actual = test plisting "_start:"

        let expected = 
            [ 
                "_start" |> Label.Label  |> ListingEntry.Label 
            ]
            |> Listing

        Assert.Equal(expected, actual)


    [<Fact>]
    let ``plisting should parse a label and comment`` () =
        let actual = test plisting """_start:
; hello"""

        let expected = 
            [ 
                "_start" |> Label.Label  |> ListingEntry.Label 
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse an empty newline`` () =

        let p = ws >>. opt comment .>> skipNewline |>> ignore

        let actual = test p """
"""


        let expected = 
            //[ ]
            //|> Listing
            ()
        
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse a whitespace and comments`` () =
        let actual = test plisting """; ehllow
     
 ; hellow"""

        let expected = 
            []
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``plisting should parse a push instruction`` () =
        let actual = test plisting "push rax"

        let expected = 
            [
                RAX |> Register64 |> Register |> pushIns
            ]
            |> Listing

        Assert.Equal(expected, actual)



    [<Fact>]
    let ``plisting should parse basic.asm`` () =
        let ``basic.asm`` = file "basic.asm"

        let actual = test plisting ``basic.asm``

        let expected = 
            [ 
                Global "_start"
                Section ".text"
                "_start" |> Label.Label  |> ListingEntry.Label 
                RAX |> Register64 |> Register |> pushIns 
                moveIns (RDI |> Register64 |> Register) (Int16 1s)
            ]
            |> Listing

        Assert.Equal(expected, actual)
