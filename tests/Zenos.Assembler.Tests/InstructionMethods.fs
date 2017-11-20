module InstructionMethods

open Zenos.Framework

let toSectionEntry inst = SectionEntry (None, inst)

let popIns = Instruction.pop >> toSectionEntry
let syscallIns = Instruction.syscall |> toSectionEntry
let pushIns = Instruction.push >> toSectionEntry
let moveIns a b = Instruction.move a b |> toSectionEntry
let xorIns a b = Instruction.xor a b |> toSectionEntry

let section name items = Section (name, items)
