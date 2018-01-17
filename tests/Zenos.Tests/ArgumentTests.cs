using Xunit;
using Xunit.Abstractions;

namespace Zenos.Tests
{
    public class ArgumentTests : TestBase
    {
        public ArgumentTests(ITestOutputHelper output)
            : base(output)
        {
        }

        private delegate int EightArgDelegate(int a, int b, int c, int d, int e, int f, int g, int h);
        private delegate int TwelveArgDelegate(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l);

        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)]
        public void Arg01(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            var runner = F<EightArgDelegate, int>(func => func(a, b, c, d, e, f, g, h));

            runner((aa, bb, cc, dd, ee, ff, gg, hh) => aa);
        }

        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)]
        public void Arg02(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            var runner = F<EightArgDelegate, int>(func => func(a, b, c, d, e, f, g, h));

            runner((aa, bb, cc, dd, ee, ff, gg, hh) => bb);
        }


        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)]
        public void Arg03(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            var runner = F<EightArgDelegate, int>(func => func(a, b, c, d, e, f, g, h));

            runner((aa, bb, cc, dd, ee, ff, gg, hh) => cc);
        }


        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)]
        public void Arg04(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            var runner = F<EightArgDelegate, int>(func => func(a, b, c, d, e, f, g, h));

            runner((aa, bb, cc, dd, ee, ff, gg, hh) => dd);
        }


        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)]
        public void Arg05(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            var runner = F<EightArgDelegate, int>(func => func(a, b, c, d, e, f, g, h));

            runner((aa, bb, cc, dd, ee, ff, gg, hh) => ee);
        }


        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)]
        public void Arg06(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            var runner = F<EightArgDelegate, int>(func => func(a, b, c, d, e, f, g, h));

            runner((aa, bb, cc, dd, ee, ff, gg, hh) => ff);
        }


        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)]
        public void Arg07(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            var runner = F<EightArgDelegate, int>(func => func(a, b, c, d, e, f, g, h));

            runner((aa, bb, cc, dd, ee, ff, gg, hh) => gg);
        }


        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007)]
        public void Arg08(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            var runner = F<EightArgDelegate, int>(func => func(a, b, c, d, e, f, g, h));

            runner((aa, bb, cc, dd, ee, ff, gg, hh) => hh);
        }

        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011)]
        public void Arg08Of12(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l)
        {
            var runner = F<TwelveArgDelegate, int>(func => func(a, b, c, d, e, f, g, h, i, j, k, l));

            runner((aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk, ll) => hh);
        }

        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011)]
        public void Arg09Of12(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l)
        {
            var runner = F<TwelveArgDelegate, int>(func => func(a, b, c, d, e, f, g, h, i, j, k, l));

            runner((aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk, ll) => ii);
        }

        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011)]
        public void Arg10Of12(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l)
        {
            var runner = F<TwelveArgDelegate, int>(func => func(a, b, c, d, e, f, g, h, i, j, k, l));

            runner((aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk, ll) => jj);
        }

        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011)]
        public void Arg11Of12(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l)
        {
            var runner = F<TwelveArgDelegate, int>(func => func(a, b, c, d, e, f, g, h, i, j, k, l));

            runner((aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk, ll) => kk);
        }

        [Theory]
        [InlineData(1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011)]
        public void Arg12Of12(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l)
        {
            var runner = F<TwelveArgDelegate, int>(func => func(a, b, c, d, e, f, g, h, i, j, k, l));

            runner((aa, bb, cc, dd, ee, ff, gg, hh, ii, jj, kk, ll) => ll);
        }
    }
}