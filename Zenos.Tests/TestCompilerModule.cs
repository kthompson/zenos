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
            this.Bind<Compiler>().To<Compiler>();
            
            this.Bind<ICompilerStage>().To<ModuleQueuingStage>();

            this.Bind<ICompilerStage>().To<CodeQueuingStage>();
            this.Bind<ICompilerStage>().To<GenerateRuntimeStage>();
            this.Bind<ICompilerStage>().To<CodeSimplifier>();
            this.Bind<ICompilerStage>().To<CilToExpressionTranslator>();

            this.Bind<ICompilerStage>().To<EmitterStage>();
            this.Bind<ICompilerStage>().To<WriteCodeToDisk>();
            this.Bind<ICompilerStage>().To<GccBuildStage>();
        }
    }
}