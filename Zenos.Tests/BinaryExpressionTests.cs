using System;
using NUnit.Framework;

namespace Zenos.Tests
{
    [TestFixture]
    public class BinaryExpressionTests
    {

        [Test]
        public void Arithmetic(
            [Random(int.MinValue, int.MaxValue, 5)] int a,
            [Random(int.MinValue, int.MaxValue, 5)] int b)
        {
            Test.Runs((aa, bb) => aa + bb, a, b);
            Test.Runs((aa, bb) => aa - bb, a, b);
            Test.Runs((aa, bb) => aa / bb, a, b);
            Test.Runs((aa, bb) => aa * bb, a, b);
        }

        [Test]
        public void Bitwise(
            [Random(int.MinValue, int.MaxValue, 5)] int a,
            [Random(int.MinValue, int.MaxValue, 5)] int b)
        {

            Test.Runs((aa, bb) => aa & bb, a, b);
            Test.Runs((aa, bb) => aa | bb, a, b);
            Test.Runs((aa, bb) => aa ^ bb, a, b);
        }

        #region Equality tests

        [Test]
        public void Equality(
            [Random(int.MinValue, int.MaxValue, 5)] int a,
            [Random(int.MinValue, int.MaxValue, 5)] int b)
        {
            Test.Runs((aa, bb) => aa > bb, a, b);
            Test.Runs((aa, bb) => aa >= bb, a, b);
            Test.Runs((aa, bb) => aa == bb, a, b);
            Test.Runs((aa, bb) => aa <= bb, a, b);
            Test.Runs((aa, bb) => aa < bb, a, b);
            Test.Runs((aa, bb) => aa != bb, a, b);
        }

        [Test]
        public void Equality(
            [Values(true, true, false, false)] bool a,
            [Values(true, false, true, false)] bool b)
        {
            Test.Runs((aa, bb) => aa == bb, a, b);
            Test.Runs((aa, bb) => aa != bb, a, b);
        }

        [Test]
        public void Equality(
            [Random(float.MinValue, float.MaxValue, 5)] float a,
            [Random(float.MinValue, float.MaxValue, 5)] float b)
        {
            Test.Runs((aa, bb) => aa > bb, a, b);
            Test.Runs((aa, bb) => aa >= bb, a, b);
            Test.Runs((aa, bb) => aa == bb, a, b);
            Test.Runs((aa, bb) => aa <= bb, a, b);
            Test.Runs((aa, bb) => aa < bb, a, b);
            Test.Runs((aa, bb) => aa != bb, a, b);
        }

        [Test]
        public void Equality(
            [Random(double.MinValue, double.MaxValue, 5)] double a,
            [Random(double.MinValue, double.MaxValue, 5)] double b)
        {
            Test.Runs((aa, bb) => aa > bb, a, b);
            Test.Runs((aa, bb) => aa >= bb, a, b);
            Test.Runs((aa, bb) => aa == bb, a, b);
            Test.Runs((aa, bb) => aa <= bb, a, b);
            Test.Runs((aa, bb) => aa < bb, a, b);
            Test.Runs((aa, bb) => aa != bb, a, b);
        }

        [Test]
        public void Equality(
            [Random(char.MinValue, char.MaxValue, 5)] char a,
            [Random(char.MinValue, char.MaxValue, 5)] char b)
        {
            Test.Runs((aa, bb) => aa > bb, a, b);
            Test.Runs((aa, bb) => aa >= bb, a, b);
            Test.Runs((aa, bb) => aa == bb, a, b);
            Test.Runs((aa, bb) => aa <= bb, a, b);
            Test.Runs((aa, bb) => aa < bb, a, b);
            Test.Runs((aa, bb) => aa != bb, a, b);
        }

        [Test]
        public void Equality(
            [Random(short.MinValue, short.MaxValue, 5)] short a,
            [Random(short.MinValue, short.MaxValue, 5)] short b)
        {
            Test.Runs((aa, bb) => aa > bb, a, b);
            Test.Runs((aa, bb) => aa >= bb, a, b);
            Test.Runs((aa, bb) => aa == bb, a, b);
            Test.Runs((aa, bb) => aa <= bb, a, b);
            Test.Runs((aa, bb) => aa < bb, a, b);
            Test.Runs((aa, bb) => aa != bb, a, b);
        }

        [Test]
        public void Equality(
            [Random(byte.MinValue, byte.MaxValue, 5)] byte a,
            [Random(byte.MinValue, byte.MaxValue, 5)] byte b)
        {
            Test.Runs((aa, bb) => aa > bb, a, b);
            Test.Runs((aa, bb) => aa >= bb, a, b);
            Test.Runs((aa, bb) => aa == bb, a, b);
            Test.Runs((aa, bb) => aa <= bb, a, b);
            Test.Runs((aa, bb) => aa < bb, a, b);
            Test.Runs((aa, bb) => aa != bb, a, b);
        }

        #endregion
    }
}
