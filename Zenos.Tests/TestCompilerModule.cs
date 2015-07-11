using System;
using System.Diagnostics;
using System.IO;
using Ninject.Modules;
using Zenos.Framework;
using Zenos.Stages;

namespace Zenos.Tests
{
    public class TestCompilerModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<Compiler>().ToSelf();

            this.Bind<IArchitecture>().To<AMD64>();
            
            //this.Bind<ICompilerStage>().To<TypeQueuingStage>();
            //this.Bind<ICompilerStage>().To<MethodQueuingStage>();
            this.Bind<ICompilerStage>().To<CecilToZenos>();
            this.Bind<ICompilerStage>().To<MethodToZil>();

            this.Bind<ICompilerStage>().To<CilCodeSimplifier>();

            this.Bind<ICompilerStage>().To<LocalPropagation>();
            this.Bind<ICompilerStage>().To<BranchOptimization>();
            this.Bind<ICompilerStage>().To<DeadCodeElimination>();

            this.Bind<ICompilerStage>().To<StaticSingleAssignmentTranslation>();
            
            this.Bind<ICompilerStage>().To<EmitterStage>();
        }
    }
}