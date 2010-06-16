using NUnit.Framework;

namespace Zenos.Tests
{
    [TestFixture]
    public class UnaryExpressionTests
    {
        [Test]
        public void UnaryLogicalNotExpression()
        {


            Test.Runs(() =>
                                                             {
                                                                 var fieldT = true;
                                                                 return !fieldT;
                                                             });
            Test.Runs(() =>
                                                            {
                                                                var fieldF = false;
                                                                return !fieldF;
                                                            });
        }

        [Test]
        public void UnaryBitwiseNotExpression()
        {
            Test.Runs(() =>
            {
                var field = 0xfffffff0;
                return ~field;
            });

            Test.Runs(() =>
            {
                var field = 0xf;
                return ~field;
            });
        }

        [Test]
        public void UnaryNegateExpression()
        {
            Test.Runs(() =>
            {
                var field = -15;
                return -field;
            });

            Test.Runs(() =>
            {
                var field = 16;
                return -field;
            });

            Test.Runs(() =>
            {
                var field = 0;
                return -field;
            });

            Test.Runs(() =>
            {
                var field = 21897;
                return -field;
            });

            Test.Runs(() =>
            {
                var field = -673;
                return -field;
            });

            Test.Runs(() =>
            {
                var field = 673;
                return -field;
            });


            Test.Runs(() =>
            {
                var field = 3.4f;
                return -field;
            });
        }
    }
}
