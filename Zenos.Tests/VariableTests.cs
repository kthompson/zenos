using Xunit;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable ConvertToConstant.Local

namespace Zenos.Tests
{
    public class VariableTests
    {
        [Fact]
        public void Bools()
        {
            var func = Test.Runs<BoolDelegate, bool>(f => f());

            func(() =>
            {
                var fieldT = true;
                var fieldF = false;
                return fieldT;
            });

            func(() =>
            {
                var fieldF = false;
                var fieldT = true;
                return fieldF;
            });
        }

        [Fact]
        public void Ints()
        {
            var func = Test.Runs<Int32Delegate, int>(f => f());

            func(() =>
            {
                var fieldF = 1;
                var fieldT = 234;
                return fieldF;
            });

            func(() =>
            {
                var fieldF = 2;
                var fieldT = 234;
                return fieldF;
            });

            func(() =>
            {
                var fieldF = 5;
                var field2 = fieldF;

                var fieldT = 234;
                return field2;
            });

            func(() =>
            {
                var fieldF = -2;
                var fieldT = 234;
                return fieldF;
            });
        }

        [Fact]
        public void Longs()
        {
            var func = Test.Runs<Int64Delegate, long>(f => f());

            func(() =>
            {
                var fieldF = 1L;
                var fieldT = 234L;
                return fieldF;
            });

            func(() =>
            {
                var fieldF = 2L;
                var fieldT = 234L;
                return fieldF;
            });

            func(() =>
            {
                var fieldF = 5L;
                var field2 = fieldF;

                var fieldT = 234L;
                return field2;
            });

            func(() =>
            {
                var fieldF = -2L;
                var fieldT = 234L;
                return fieldF;
            });
        }

        [Fact]
        public void Chars()
        {
            var func = Test.Runs<CharDelegate, char>(f => f());

            func(() =>
            {
                var fieldF = 'a';
                var fieldT = 'l';
                return fieldF;
            });

            func(() =>
            {
                var fieldF = 'a';
                var field2 = fieldF;
                var fieldT = 234;
                return field2;
            });
        }

        [Fact]
        public void Floats()
        {
            var func = Test.Runs<SingleDelegate, float>(f => f());

            func(() =>
            {
                var fieldF = 3.14f;
                var fieldT = 1.2f;
                return fieldF;
            });

            func(() =>
            {
                var fieldF = 1.2f;
                var fieldT = 3.14f;
                return fieldF;
            });
        }

        [Fact]
        public void Doubles()
        {
            var func = Test.Runs<DoubleDelegate, double>(f => f());

            func(() =>
            {
                var fieldF = 3.14;
                var fieldT = 1.2;
                return fieldF;
            });

            func(() =>
            {
                var fieldF = 1.2;
                var fieldT = 3.14;
                return fieldF;
            });
        }
    }
}
