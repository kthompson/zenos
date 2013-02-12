﻿using System;
using System.Diagnostics;
using System.IO;
using Mono.Cecil;
using Ninject.Modules;
using Zenos.Framework;
using Zenos.Stages;

namespace Zenos.Tests
{
    public class TestCompilerModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICompiler>().To<Compiler>();
            this.Bind<CompilerStage>().To<ModuleQueuingStage>();
            this.Bind<CompilerStage>().To<MemberCompilerCompilerStage>();
            this.Bind<CompilerStage>().To<GccBuildStage>();

            this.Bind<IMemberCompiler>().To<MemberCompiler>();
            this.Bind<MemberCompilerStage>().To<GenerateRuntimeStage>();
            this.Bind<MemberCompilerStage>().To<CodeQueuingStage>();

            this.Bind<ICodeCompiler>().To<CodeCompiler>();
            this.Bind<CodeCompilerStage>().To<CodeSimplifier>();
            this.Bind<CodeCompilerStage>().To<EmitterStage>();
            this.Bind<CodeCompilerStage>().To<WriteCodeToDisk>();
        }
    }

    public class MemberCompilerCompilerStage : CompilerStage
    {

    }
}