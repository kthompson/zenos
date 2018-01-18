using Xunit;
using Xunit.Abstractions;

namespace Zenos.Tests
{
    public class UnaryExpressionTests : TestBase
    {

        [Fact]
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

        [Fact]
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

        [Fact]
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
        }


        [Fact(Skip = "add float support")]
        public void UnaryNegateFloatExpression()
        {
            Ffloat(() =>
            {
                var field = 3.4f;
                return -field;
            });
        }

        public UnaryExpressionTests(ITestOutputHelper output)
            : base(output)
        {
        }
    }
}
