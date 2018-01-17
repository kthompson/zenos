namespace Zenos.Assembler.Tests

open Xunit
open Xunit.Abstractions

open System
open System.IO

open FParsec
open Zenos.Framework
open Zenos.Framework.AssemblyParsing

module EmitterTests =
    open InstructionMethods
    
    [<Fact>]
    let ``should emit blah`` () = 
        let listing = 
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
            
        //Assert.Equal(expected, actual)
        ()

