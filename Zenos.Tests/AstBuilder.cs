using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Zenos.Framework;
using Zenos.Stages;

namespace Zenos.Tests
{
    class AstBuilder
    {
    }

    public class AstBuilderModule : NinjectModule
    {
        public override void Load()
        {
            /*
            this.Bind<ICompilerStage>().To<Compiler>();
            this.Bind<CompilerStage>().To<ModuleQueuingStage>();

            this.Bind<IMemberCompilerStage>().To<MemberCompiler>();
            this.Bind<MemberCompilerStage>().To<CodeQueuingStage>();
            
            this.Bind<CodeCompilerStage>().To<CodeSimplifier>();
            this.Bind<CodeCompilerStage>().To<EmitterStage>();
            this.Bind<CodeCompilerStage>().To<WriteCodeToDisk>();
            */
        }
    }
}
