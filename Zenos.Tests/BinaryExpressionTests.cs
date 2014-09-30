using System;
using Xunit;

namespace Zenos.Tests
{
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

        [Theory]
        [PermuteData(int.MaxValue, int.MinValue, 0, 123654, 1236234674, 999999999, 50, 27)]
        public void Arithmetic(int a, int b)
        {
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa + bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa - bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa/bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa*bb, a, b);
        }


        [Theory]
        [PermuteData(int.MaxValue, int.MinValue, 0, 123654, 1236234674, 999999999, 50, 27)]
        public void Bitwise(int a, int b)
        {

            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa & bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa | bb, a, b);
            Test.Runs<BinaryExpressionDelegate>((aa, bb) => aa ^ bb, a, b);
        }

        #region Equality tests

        [Theory]
        [PermuteData(int.MaxValue, int.MinValue, 0, 123654, 1236234674, 999999999, 50, 27)]
        public void Equality(int a, int b)
        {
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryIntBoolExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void Equality(bool a, bool b)
        {
            Test.Runs<BinaryBoolExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryBoolExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Theory]
        [PermuteData(Single.MaxValue, Single.MinValue,
            0.5f, 12365, 123.74f, 9999, 50.1321654f,
            2720938902384902834.034234234234323423f)]
        public void Equality(float a, float b)
        {
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryFloatExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Theory]
        [PermuteData(Double.MaxValue, Double.MinValue,
            0.5D, 12365D, 123.74D, 9999D, 50.1321654D,
            2720938902384902834.034234234234323422334234234234234234234230989080890812321313D)]
        public void Equality(double a, double b)
        {
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryDoubleExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Theory]
        [PermuteData(char.MaxValue, char.MinValue, 'a', '0', 'z', 'A')]
        public void Equality(char a, char b)
        {
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryCharExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Theory]
        [PermuteData(short.MaxValue, short.MinValue, 0, 12365, 12374, 9999, 50, 27)]
        public void Equality(short a, short b)
        {
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa > bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa >= bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa == bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa <= bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa < bb, a, b);
            Test.Runs<BinaryInt16ExpressionDelegate>((aa, bb) => aa != bb, a, b);
        }

        [Theory]
        [PermuteData(byte.MinValue, 50, 59, 90, byte.MaxValue)]
        public void Equality(byte a, byte b)
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
