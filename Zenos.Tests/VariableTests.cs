using NUnit.Framework;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable ConvertToConstant.Local

namespace Zenos.Tests
{
    [TestFixture]
    public class VariableTests
    {
        [Test]
        public void Bools()
        {
            Test.Runs(() =>
            {
                var fieldT = true;
                var fieldF = false;
                return fieldT;
            });

            Test.Runs(() =>
            {
                var fieldF = false;
                var fieldT = true;
                return fieldF;
            });
        }

        [Test]
        public void Ints()
        {

            Test.Runs(() =>
            {
                var fieldF = 1;
                var fieldT = 234;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 2;
                var fieldT = 234;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 5;
                var field2 = fieldF;

                var fieldT = 234;
                return field2;
            });

            Test.Runs(() =>
            {
                var fieldF = -2;
                var fieldT = 234;
                return fieldF;
            });
        }

        [Test]
        public void Longs()
        {

            Test.Runs(() =>
            {
                var fieldF = 1L;
                var fieldT = 234L;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 2L;
                var fieldT = 234L;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 5L;
                var field2 = fieldF;

                var fieldT = 234L;
                return field2;
            });

            Test.Runs(() =>
            {
                var fieldF = -2L;
                var fieldT = 234L;
                return fieldF;
            });
        }

        [Test]
        public void Chars()
        {
            Test.Runs(() =>
            {
                var fieldF = 'a';
                var fieldT = 'l';
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 'a';
                var field2 = fieldF;
                var fieldT = 234;
                return field2;
            });
        }

        [Test]
        public void Floats()
        {
            Test.Runs(() =>
            {
                var fieldF = 3.14f;
                var fieldT = 1.2f;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 1.2f;
                var fieldT = 3.14f;
                return fieldF;
            });
        }

        [Test]
        public void Doubles()
        {
            Test.Runs(() =>
            {
                var fieldF = 3.14;
                var fieldT = 1.2;
                return fieldF;
            });

            Test.Runs(() =>
            {
                var fieldF = 1.2;
                var fieldT = 3.14;
                return fieldF;
            });
        }
    }
}
