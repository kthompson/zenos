using System;
using System.Runtime.CompilerServices;
using Zenos.Framework;
using Xunit;
using Xunit.Abstractions;

namespace Zenos.Tests
{
    public class TestBase
    {
		protected readonly TestRunner TestRunner;
		protected readonly ITestOutputHelper output;

		protected TestBase(ITestOutputHelper output)
		{
			this.output = output;
			this.TestRunner = new TestRunner(output);
		}

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static char CharDelegateExecuter(CharDelegate f) => f();
        protected void Fchar(CharDelegate func, Action<MethodContext> action = null)
        {
            F<CharDelegate, char>(CharDelegateExecuter, action)(func);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool BoolDelegateExecuter(BoolDelegate f) => f();
        protected void Fbool(BoolDelegate func, Action<MethodContext> action = null)
        {
            F<BoolDelegate, bool>(BoolDelegateExecuter, action)(func);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static uint UInt32DelegateExecuter(UInt32Delegate f) => f();
        protected void Fuint(UInt32Delegate func, Action<MethodContext> action = null)
        {
            F<UInt32Delegate, uint>(UInt32DelegateExecuter, action)(func);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int Int32DelegateExecuter(Int32Delegate f) => f();
        protected void Fint(Int32Delegate func, Action<MethodContext> action = null)
        {
            F<Int32Delegate, int>(Int32DelegateExecuter, action)(func);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ulong UInt64DelegateExecuter(UInt64Delegate f) => f();
        protected void Fulong(UInt64Delegate func, Action<MethodContext> action = null)
        {
            F<UInt64Delegate, ulong>(UInt64DelegateExecuter, action)(func);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static long Int64DelegateExecuter(Int64Delegate f) => f();
        protected void Flong(Int64Delegate func, Action<MethodContext> action = null)
        {
            F<Int64Delegate, long>(Int64DelegateExecuter, action)(func);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static float SingleDelegateExecuter(SingleDelegate f) => f();
        protected void Ffloat(SingleDelegate func, Action<MethodContext> action = null)
        {
            F<SingleDelegate, float>(SingleDelegateExecuter, action)(func);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static double DoubleDelegateExecuter(DoubleDelegate f) => f();
        protected void Fdouble(DoubleDelegate func, Action<MethodContext> action = null)
        {
            F<DoubleDelegate, double>(DoubleDelegateExecuter, action)(func);
        }

        protected Action<TDelegate> F<TDelegate, TResult>(Func<TDelegate, TResult> func, Action<MethodContext> action = null)
            where TDelegate : class
        {
            return TestRunner.Runs(func, Assert.Equal, action);
        }
    }
}


