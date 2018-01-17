namespace Zenos.Framework

open Zenos.Stages

module CompilerBuilder =
    let testCompiler out = 
        [
            CompilerStages.printer out
            CompilerStages.cecilToX86
            CompilerStages.printer out
            CompilerStages.emit
        ]
        |> Compiler.createStaged

