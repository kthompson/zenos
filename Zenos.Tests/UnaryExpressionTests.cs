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


            Test.Runs<BoolDelegate>(() =>
                                                             {
                                                                 var fieldT = true;
                                                                 return !fieldT;
                                                             });
            Test.Runs<BoolDelegate>(() =>
                                                            {
                                                                var fieldF = false;
                                                                return !fieldF;
                                                            });
        }

        [Fact]
        public void UnaryBitwiseNotExpression()
        {
            Test.Runs<UInt32Delegate>(() =>
            {
                var field = 0xfffffff0;
                return ~field;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var field = 0xf;
                return ~field;
            });
        }

        [Fact]
        public void UnaryNegateExpression()
        {
            Test.Runs<Int32Delegate>(() =>
            {
                var field = -15;
                return -field;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var field = 16;
                return -field;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var field = 0;
                return -field;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var field = 21897;
                return -field;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var field = -673;
                return -field;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var field = 673;
                return -field;
            });


            Test.Runs<SingleDelegate>(() =>
            {
                var field = 3.4f;
                return -field;
            });
        }
    }
}
