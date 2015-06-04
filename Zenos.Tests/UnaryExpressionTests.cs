using System;
using Xunit;

namespace Zenos.Tests
{
    delegate bool BoolDelegate();
    delegate uint UInt32Delegate();
    delegate int Int32Delegate();
    delegate long Int64Delegate();
    delegate float SingleDelegate();
    delegate double DoubleDelegate();
    delegate char CharDelegate();

    public class UnaryExpressionTests
    {

        [Fact]
        public void UnaryLogicalNotExpression()
        {
            var func = Test.Runs<BoolDelegate, bool>(f => f());

            func(() =>
            {
                var fieldT = true;
                return !fieldT;
            });

            func(() =>
            {
                var fieldF = false;
                return !fieldF;
            });
        }

        [Fact]
        public void UnaryBitwiseNotExpression()
        {
            Test.Runs<UInt32Delegate, uint>(f => f())(() =>
            {
                var field = 0xfffffff0;
                return ~field;
            });

            Test.Runs<Int32Delegate, int>(f => f())(() =>
            {
                var field = 0xf;
                return ~field;
            });
        }

        [Fact]
        public void UnaryNegateExpression()
        {
            var func = Test.Runs<Int32Delegate, int>(f => f());

            func(() =>
            {
                var field = -15;
                return -field;
            });

            func(() =>
            {
                var field = 16;
                return -field;
            });

            func(() =>
            {
                var field = 0;
                return -field;
            });

            func(() =>
            {
                var field = 21897;
                return -field;
            });

            func(() =>
            {
                var field = -673;
                return -field;
            });

            func(() =>
            {
                var field = 673;
                return -field;
            });

            Test.Runs<SingleDelegate, float>(f => f())(() =>
            {
                var field = 3.4f;
                return -field;
            });
        }
    }
}
