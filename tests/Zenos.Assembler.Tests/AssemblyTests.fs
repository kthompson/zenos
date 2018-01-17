namespace Zenos.Assembler.Tests

open Xunit
open Xunit.Abstractions

open System
open System.IO

open FParsec
open Zenos.Framework
open Zenos.Framework.AssemblyParsing
open InstructionMethods

type AssemblyTests(output:ITestOutputHelper) =

    let file name = 
        File.ReadAllText(Path.Combine("resources", name))

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
    let ``pListing should parse empty input`` () =
        let actual = test pListing ""

        let expected = 
            []
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse comments-only.asm`` () =
        let ``comments-only.asm`` = file "comments-only.asm"

        let actual = test pListing ``comments-only.asm``

        let expected = 
            []
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse hello.asm`` () =
        let ``hello.asm`` = file "hello.asm"

        let actual = test pListing ``hello.asm``

        let expected = 
            [ 
                Global "_start"
                section ".text" <| [
                    "_start" |> Label.Label  |> SectionEntry.Label
                
                    moveIns (RAX |> Register64 |> Register) (ImmediateInt16 1s |> Immediate )
                    moveIns (RDI |> Register64 |> Register) (ImmediateInt16 1s |> Immediate )
                    moveIns (RSI |> Register64 |> Register) (Operand.LabelReference "message")
                    moveIns (RDX |> Register64 |> Register) (ImmediateInt16 13s |> Immediate )
                    syscallIns

                    moveIns (EAX |> Register32 |> Register) (ImmediateInt16 60s |> Immediate )
                    xorIns (RDI |> Register64 |> Register) (RDI |> Register64 |> Register)                
                    syscallIns
                    "message" |> Label.Label  |> SectionEntry.Label
                    // db      "Hello, World", 10      ; note the newline at the end
                    Data8 [ DataString "Hello, World"; DataInt 10L ] |> toSectionEntry 
                ]
            ]
            |> Listing
            
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse globals`` () =
        let actual = test pListing "global _start"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse global with whitespace`` () =
        let actual = test pListing "  global _start"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing  

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse global with whitespace and comment`` () =
        let actual = test pListing "  global _start; bleh"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)


    [<Fact>]
    let ``pListing should parse global with whitespace and comment2`` () =
        let actual = test pListing "  global _start ; bleh"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)


    [<Fact>]
    let ``pListing should parse global with comment2`` () =
        let actual = test pListing "global _start ; bleh"

        let expected = 
            [ 
                Global "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)


    [<Fact>]
    let ``pListing should parse extern`` () =
        let actual = test pListing """extern _start"""

        let expected = 
            [ 
                Extern "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse extern with newLine`` () =
        let actual = test pListing """extern _start
"""

        let expected = 
            [ 
                Extern "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse extern twice`` () =
        let str = """extern first
extern second"""
        let actual = test pListing str

        let expected = 
            [ 
                Extern "first"
                Extern "second"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse global and extern`` () =
        let actual = test pListing """global _start
extern _start"""

        let expected = 
            [ 
                Global "_start"
                Extern "_start"
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse a label`` () =
        let actual = test pListing "_start:"

        let expected = 
            [ 
                section ".text" <| [
                    ("_start" |> Label.Label  |> SectionEntry.Label)
                ]
            ]
            |> Listing

        Assert.Equal(expected, actual)


    [<Fact>]
    let ``pListing should parse a label and comment`` () =
        let actual = test pListing """_start:
; hello"""

        let expected = 
            [ 
                section ".text" <| [
                    "_start" |> Label.Label  |> SectionEntry.Label 
                ]
            ]
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse an empty newline`` () =

        let actual = test pListing """
"""

        let expected = Listing [ ]
        
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse a whitespace and comments`` () =
        let actual = test pListing """; ehllow
     
 ; hellow"""

        let expected = 
            []
            |> Listing

        Assert.Equal(expected, actual)

    [<Fact>]
    let ``pListing should parse a push instruction`` () =
        let actual = test pListing "push rax"

        let expected =
            [
                section ".text" <| [
                    RAX |> Register64 |> Register |> pushIns
                ]
            ]
            |> Listing

        Assert.Equal(expected, actual)



    [<Fact>]
    let ``pListing should parse basic.asm`` () =
        let ``basic.asm`` = file "basic.asm"

        let actual = test pListing ``basic.asm``

        let expected = 
            [ 
                Global "_start"
                section ".text" <| [
                     "_start" |> Label.Label |> SectionEntry.Label;
                    RAX |> Register64 |> Register |> pushIns;
                    moveIns (RDI |> Register64 |> Register) (ImmediateInt16 1s |> Immediate )
                ]
            ]
            |> Listing

        Assert.Equal(expected, actual)
