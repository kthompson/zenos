using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Zenos.Framework
{
    public interface ICompiler<TContext>
    {
        TContext Compile(TContext context);
    }

    public class Compiler<TContext> : ICompiler<TContext>
    {
        public virtual TContext Compile(TContext context) => context;
    }

    public class StagedCompiler<TContext> : Compiler<TContext>
    {
        private readonly ICompiler<TContext>[] _stages;

        public StagedCompiler(params ICompiler<TContext>[] stages)
        {
            _stages = stages;
        }

        public override TContext Compile(TContext context) =>
            _stages.Aggregate(context, (acc, compiler) => compiler.Compile(acc));
    }

    public static class Compiler
    {
        class AnonymousCompiler<TContext> : Compiler<TContext>
        {
            private readonly Func<TContext, TContext> _compiler;

            public AnonymousCompiler(Func<TContext, TContext> compiler)
            {
                _compiler = compiler;
            }

            public override TContext Compile(TContext context)
            {
                return _compiler(context);
            }
        }

        public static ICompiler<TContext> Create<TContext>(Func<TContext, TContext> compiler) => 
            new AnonymousCompiler<TContext>(compiler);

        public static ICompiler<TContext> CreateStaged<TContext>(params ICompiler<TContext>[] stages) =>
            new StagedCompiler<TContext>(stages);

        public static ICompiler<TContext> CreateStagedWithBefore<TContext>(
                Action<ICompiler<TContext>, TContext> before,
                params ICompiler<TContext>[] stages) =>
            CreateStagedWithBeforeAndAfter(before, null, stages);

        public static ICompiler<TContext> CreateStagedWithAfter<TContext>(
                Action<ICompiler<TContext>, TContext> after,
                params ICompiler<TContext>[] stages) =>
            CreateStagedWithBeforeAndAfter(null, after, stages);

        public static ICompiler<TContext> CreateStagedWithBeforeAndAfter<TContext>(
            Action<ICompiler<TContext>, TContext> before,
            Action<ICompiler<TContext>, TContext> after,
            params ICompiler<TContext>[] stages)
        {
            if (before == null && after == null)
                return new StagedCompiler<TContext>(stages.ToArray());

            if (before == null)
            {
                var newStages =
                    stages.Select(stage => Create<TContext>(context =>
                    {
                        var result = stage.Compile(context);
                        after(stage, result);
                        return result;
                    }));

                return new StagedCompiler<TContext>(newStages.ToArray());
            }

            if (after == null)
            {
                var newStages =
                    from stage in stages
                    select Create<TContext>(context =>
                    {
                        before(stage, context);
                        return stage.Compile(context);
                    });

                return new StagedCompiler<TContext>(newStages.ToArray());
            }
            else
            {
                var newStages =
                    from stage in stages
                    select Create<TContext>(context =>
                    {
                        before(stage, context);
                        var result = stage.Compile(context);
                        after(stage, result);
                        return result;
                    });
                return new StagedCompiler<TContext>(newStages.ToArray());
            }
        }

    }
}
