using NUnit.Framework;

namespace Zenos.Tests
{
    [TestFixture]
    public class VariableTests
    {
        [Test]
        public void VariableReferenceTests()
        {
            Test.Runs(() =>
            {
                var fieldT = true;
                return fieldT;
            });

            Test.Runs(() =>
            {
                var fieldF = false;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 1;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 2;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 'a';
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 5;
                var field2 = fieldF;
                return field2;
            });

            Test.Runs(() =>
            {
                var fieldF = 'a';
                var field2 = fieldF;
                return field2;
            });

            Test.Runs(() =>
            {
                var fieldF = -2;
                return fieldF;
            });
        }
    }
}
