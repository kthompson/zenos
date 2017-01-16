using System;
using Xunit;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable ConvertToConstant.Local
#pragma warning disable 219

namespace Zenos.Tests
{
    public class VariableTests : TestBase
    {
        [Fact]
        public void Bools()
        {
            Fbool(() =>
            {
                var fieldT = true;
                var fieldF = false;
                return fieldT;
            });

            Fbool(() =>
            {
                var fieldF = false;
                var fieldT = true;
                return fieldF;
            });
        }

        [Fact]
        public void Ints()
        {
            Fint(() =>
            {
                var fieldF = 1;
                var fieldT = 234;
                return fieldF;
            });

            Fint(() =>
            {
                var fieldF = 2;
                var fieldT = 234;
                return fieldF;
            });

            Fint(() =>
            {
                var fieldF = 5;
                var field2 = fieldF;

                var fieldT = 234;
                return field2;
            });

            Fint(() =>
            {
                var fieldF = -2;
                var fieldT = 234;
                return fieldF;
            });
        }

        [Fact]
        public void Longs()
        {
            Flong(() =>
            {
                var fieldF = 1L;
                var fieldT = 234L;
                return fieldF;
            });

            Flong(() =>
            {
                var fieldF = 5L;
                var field2 = fieldF;

                var fieldT = 234L;
                return field2;
            });

            Flong(() =>
            {
                var fieldF = -2L;
                var fieldT = 234L;
                return fieldF;
            });
        }

        [Fact]
        public void Chars()
        {
            Fchar(() =>
            {
                var fieldF = 'a';
                var fieldT = 'l';
                return fieldF;
            });

            Fchar(() =>
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
            Ffloat(() =>
            {
                var fieldF = 3.14f;
                var fieldT = 1.2f;
                return fieldF;
            });

            Ffloat(() =>
            {
                var fieldF = 1.2f;
                var fieldT = 3.14f;
                return fieldF;
            });
        }

        [Fact]
        public void Doubles()
        {
            Fdouble(() =>
            {
                var fieldF = 3.14;
                var fieldT = 1.2;
                return fieldF;
            });

            Fdouble(() =>
            {
                var fieldF = 1.2;
                var fieldT = 3.14;
                return fieldF;
            });
        }
    }
}
