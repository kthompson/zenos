using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using Zenos.Framework;
using Zenos.Stages;

namespace Zenos.Tests
{
    class AstBuilder
    {
        static AstBuilder()
        {
            Container = new StandardKernel(new AstBuilderModule());
        }

        private static readonly IKernel Container;
        private static Compiler Compiler
        {
            get
            {
                return Container.Get<Compiler>();
            }
        }
    }

    public class AstBuilderModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<Compiler>().To<Compiler>();

            //create member contexts for each type
            this.Bind<ICompilerStage>().To<ModuleQueuingStage>();

            //Create CompilationContext for each method
            this.Bind<ICompilerStage>().To<CodeQueuingStage>();

            this.Bind<ICompilerStage>().To<CodeSimplifier>();
            this.Bind<ICompilerStage>().To<CilToExpressionTranslator>();
            
            this.Bind<ICompilerStage>().To<EmitterStage>();
            this.Bind<ICompilerStage>().To<WriteCodeToDisk>();
            this.Bind<ICompilerStage>().To<GccBuildStage>();
        }
    }
}
