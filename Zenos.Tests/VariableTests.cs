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
            Test.Runs<BoolDelegate>(() =>
            {
                var fieldT = true;
                var fieldF = false;
                return fieldT;
            });

            Test.Runs<BoolDelegate>(() =>
            {
                var fieldF = false;
                var fieldT = true;
                return fieldF;
            });
        }

        [Test]
        public void Ints()
        {

            Test.Runs<Int32Delegate>(() =>
            {
                var fieldF = 1;
                var fieldT = 234;
                return fieldF;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var fieldF = 2;
                var fieldT = 234;
                return fieldF;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var fieldF = 5;
                var field2 = fieldF;

                var fieldT = 234;
                return field2;
            });

            Test.Runs<Int32Delegate>(() =>
            {
                var fieldF = -2;
                var fieldT = 234;
                return fieldF;
            });
        }

        [Test]
        public void Longs()
        {

            Test.Runs<Int64Delegate>(() =>
            {
                var fieldF = 1L;
                var fieldT = 234L;
                return fieldF;
            });

            Test.Runs<Int64Delegate>(() =>
            {
                var fieldF = 2L;
                var fieldT = 234L;
                return fieldF;
            });

            Test.Runs<Int64Delegate>(() =>
            {
                var fieldF = 5L;
                var field2 = fieldF;

                var fieldT = 234L;
                return field2;
            });

            Test.Runs<Int64Delegate>(() =>
            {
                var fieldF = -2L;
                var fieldT = 234L;
                return fieldF;
            });
        }

        [Test]
        public void Chars()
        {
            Test.Runs<CharDelegate>(() =>
            {
                var fieldF = 'a';
                var fieldT = 'l';
                return fieldF;
            });

            Test.Runs<CharDelegate>(() =>
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
            Test.Runs<SingleDelegate>(() =>
            {
                var fieldF = 3.14f;
                var fieldT = 1.2f;
                return fieldF;
            });

            Test.Runs<SingleDelegate>(() =>
            {
                var fieldF = 1.2f;
                var fieldT = 3.14f;
                return fieldF;
            });
        }

        [Test]
        public void Doubles()
        {
            Test.Runs<DoubleDelegate>(() =>
            {
                var fieldF = 3.14;
                var fieldT = 1.2;
                return fieldF;
            });

            Test.Runs<DoubleDelegate>(() =>
            {
                var fieldF = 1.2;
                var fieldT = 3.14;
                return fieldF;
            });
        }
    }
}
