using Xunit;

namespace Zenos.Tests
{
    public delegate bool BoolDelegate();

    public delegate uint UInt32Delegate();
    public delegate int Int32Delegate();
    public delegate long Int64Delegate();
    public delegate ulong UInt64Delegate();
    public delegate float SingleDelegate();
    public delegate double DoubleDelegate();
    public delegate char CharDelegate();

    public class UnaryExpressionTests : TestBase
    {

        public UnaryExpressionTests()
        {
        }

        [Fact(Skip = "Need to add CilCeq support")]
        public void UnaryLogicalNotExpression()
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            // ReSharper disable ConvertToConstant.Local
            Fbool(() =>
            {
                var fieldT = true;
                return !fieldT;
            });

            Fbool(() =>
            {
                var fieldF = false;
                return !fieldF;
            });
            // ReSharper restore ConvertToConstant.Local
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        [Fact(Skip = "Need to add CilNot support")]
        public void UnaryBitwiseNotExpression()
        {
            Fuint(() =>
            {
                var field = 0xfffffff0;
                return ~field;
            });

            Fint(() =>
            {
                var field = 0xf;
                return ~field;
            });
        }

        [Fact(Skip = "Need to add CilNeg support")]
        public void UnaryNegateExpression()
        {
            Fint(() =>
            {
                var field = -15;
                return -field;
            });

            Fint(() =>
            {
                var field = 16;
                return -field;
            });

            Fint(() =>
            {
                var field = 0;
                return -field;
            });

            Fint(() =>
            {
                var field = 21897;
                return -field;
            });

            Fint(() =>
            {
                var field = -673;
                return -field;
            });

            Fint(() =>
            {
                var field = 673;
                return -field;
            });

            Ffloat(() =>
            {
                var field = 3.4f;
                return -field;
            });
        }
    }
}
