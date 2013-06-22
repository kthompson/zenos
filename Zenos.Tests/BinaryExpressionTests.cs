using System;
using NUnit.Framework;

namespace Zenos.Tests
{
    [TestFixture]
    public class BinaryExpressionTests
    {
        private delegate int BinaryExpressionDelegate(int a, int b);
        private delegate bool BinaryIntBoolExpressionDelegate(int a, int b);
        private delegate bool BinaryBoolExpressionDelegate(bool a, bool b);
        private delegate bool BinaryFloatExpressionDelegate(float a, float b);
        private delegate bool BinaryDoubleExpressionDelegate(float a, float b);
        private delegate bool BinaryByteExpressionDelegate(byte a, byte b);
        private delegate bool BinaryInt16ExpressionDelegate(short a, short b);
        private delegate bool BinaryCharExpressionDelegate(float a, float b);

        [Test, Combinatorial]
        public void Arithmetic(
            [ValueSource(typeof(Data), "GetInt32s")] int a,
            [ValueSource(typeof(Data), "GetInt32s")] int b)
        {
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa + bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa - bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa / bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa * bb, a, b);
        }

        [Test, Combinatorial]
        public void Bitwise(
            [ValueSource(typeof(Data), "GetInt32s")] int a,
            [ValueSource(typeof(Data), "GetInt32s")] int b)
        {

            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa & bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa | bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa ^ bb, a, b);
        }

        #region Equality tests

        [Test, Combinatorial]
        public void Equality(
            [ValueSource(typeof(Data), "GetInt32s")] int a,
            [ValueSource(typeof(Data), "GetInt32s")] int b)
        {
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Test, Combinatorial]
        public void Equality(
            [Values(true, false)] bool a,
            [Values(true, false)] bool b)
        {
            Test.Runs<BinaryBoolExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryBoolExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Test, Combinatorial]
        public void Equality(
            [ValueSource(typeof(Data), "GetSingles")] float a,
            [ValueSource(typeof(Data), "GetSingles")] float b)
        {
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Test, Combinatorial]
        public void Equality(
            [ValueSource(typeof(Data), "GetDoubles")] double a,
            [ValueSource(typeof(Data), "GetDoubles")] double b)
        {
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Test, Combinatorial]
        public void Equality(
            [ValueSource(typeof(Data), "GetChars")] char a,
            [ValueSource(typeof(Data), "GetChars")] char b)
        {
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Test, Combinatorial]
        public void Equality(
            [ValueSource(typeof(Data), "GetInt16s")] short a,
            [ValueSource(typeof(Data), "GetInt16s")] short b)
        {
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Test, Combinatorial]
        public void Equality(
            [ValueSource(typeof(Data), "GetBytes")] byte a,
            [ValueSource(typeof(Data), "GetBytes")] byte b)
        {
            Test.Runs<BinaryByteExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryByteExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryByteExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryByteExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryByteExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryByteExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        #endregion
    }
}
