using System;
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

            this.Bind<CompilerStage>().To<CodeQueuingStage>(); 
            this.Bind<CompilerStage>().To<GenerateRuntimeStage>();
            this.Bind<CompilerStage>().To<CodeSimplifier>();
            this.Bind<CompilerStage>().To<EmitterStage>();
            this.Bind<CompilerStage>().To<WriteCodeToDisk>();
            this.Bind<CompilerStage>().To<GccBuildStage>();
        }
    }
}