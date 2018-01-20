using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenos.Tests
{
    public delegate int EightArgDelegate(int a, int b, int c, int d, int e, int f, int g, int h);
    public delegate int TwelveArgDelegate(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l);

    public delegate bool BoolDelegate();
    public delegate uint UInt32Delegate();
    public delegate int Int32Delegate();
    public delegate long Int64Delegate();
    public delegate ulong UInt64Delegate();
    public delegate float SingleDelegate();
    public delegate double DoubleDelegate();
    public delegate char CharDelegate();

    public delegate int BinaryExpressionDelegate(int a, int b);
    public delegate int TrinaryExpressionDelegate(int a, int b, int c);
    public delegate long BinaryInt64ExpressionDelegate(long a, long b);

    public delegate bool BinaryIntBoolExpressionDelegate(int a, int b);
    public delegate bool BinaryBoolExpressionDelegate(bool a, bool b);
    public delegate bool BinaryFloatExpressionDelegate(float a, float b);
    public delegate bool BinaryDoubleExpressionDelegate(double a, double b);
    public delegate bool BinaryByteExpressionDelegate(byte a, byte b);
    public delegate bool BinaryInt16ExpressionDelegate(short a, short b);
    public delegate bool BinaryCharExpressionDelegate(char a, char b);

}
