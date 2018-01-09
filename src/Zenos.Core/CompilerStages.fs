namespace Zenos.Stages

open Zenos.Core
open Zenos.Framework

module CompilerStages =
    open Compiler

    let cecilToZenos : Compiler<MethodContext> = Empty

    let branchOptimization : Compiler<MethodContext> = Empty
    let deadCodeElimination : Compiler<MethodContext> = Empty
    let localPropagation : Compiler<MethodContext> = Empty
    let staticSingleAssignmentTranslation : Compiler<MethodContext> = Empty
    let printer : Compiler<MethodContext> = Empty

