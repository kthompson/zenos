using System;
using System.Diagnostics;
using System.IO;
using Autofac;
using Zenos.Framework;
using Zenos.Stages;

namespace Zenos.Tests
{
    public class TestCompilerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(context => new Compiler(context.Resolve<MemberCompiler>()))
                   .OnActivating(context =>
                   {
                       context.Instance.Stages.Add(new ModuleQueuingStage(context.Instance));
                       context.Instance.Stages.Add(new GccBuildStage(context.Instance));
                   })
                   .SingleInstance();

            builder.Register(context => new MemberCompiler(context.Resolve<CodeCompiler>()))
                   .OnActivating(context =>
                   {
                       context.Instance.Stages.Add(new CodeQueuingStage(context.Instance));
                       context.Instance.Stages.Add(new GenerateRuntimeStage(context.Instance));
                   })
                   .SingleInstance();

            builder.Register(context => new CodeCompiler())
                   .OnActivating(context =>
                   {
                       context.Instance.Stages.Add(new CodeSimplifier(context.Instance));
                       context.Instance.Stages.Add(new EmitterStage(context.Instance));
                       context.Instance.Stages.Add(new WriteCodeToDisk(context.Instance));
                   })
                   .SingleInstance();
        }
    }
}