using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace Zenos.Tests
{
    public class ImmediateTests : TestBase
    {
        [Fact]
        public void Integers()
        {
            Fint(() => 0);
            Fint(() => 1);
            Fint(() => -1);
            Fint(() => 10);
            Fint(() => -10);
            Fint(() => 2736);
            Fint(() => -2736);
            Fint(() => 536870911);
            Fint(() => -536870912);
        }

        [Fact]
        public void Longs()
        {
            Flong(() => 1234567891011120L, context =>
            {
                var expected = new List<byte>
                {
                    0x55,                                                       // pushq   %rbp
                    0x48, 0x89, 0xe5,                                           // movq    %rsp, %rbp
                    0x48, 0xb8, 0x30, 0x46, 0x98, 0x3c, 0xd5, 0x62, 0x04, 0x00, // movabsq $-1234567891011120, %rax
                    0x5d,                                                       // popq    %rbp
                    0xc3,                                                       // retq   
                };

                var actual = context.Code;
                AssertSequenceDetailed(expected, actual);

                Assert.Equal(expected, actual);
            });
            Flong(() => -5368709121234L);
            Flong(() => 429496121113456735L);
        }

        private static void AssertSequenceDetailed<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual)
        {
            if (actual.SequenceEqual(expected)) 
                return;

            var min = Math.Min(actual.Count, expected.Count);
            for (var i = 0; i < min; i++)
            {
                var diff = Equals(expected[i], actual[i]) ? "" : " !!!!";
                Trace.WriteLine(string.Format("[{0,3}] - Expected: {1,3}, Got: {2,3}{3}", i, expected[i],
                    actual[i], diff));
            }

            Assert.True(false, "Sequence not equal");
        }

        [Fact]
        public void Boolean()
        {
            Fbool(() => true);
            Fbool(() => false);
        }

        [Fact]
        public void Characters()
        {
            Fchar(() => 'a');
            Fchar(() => 'b');
            Fchar(() => 'c');
            Fchar(() => 'd');
            Fchar(() => 'e');
            Fchar(() => 'f');

            Fchar(() => '0');
            Fchar(() => '1');
            Fchar(() => '2');
            Fchar(() => '3');
            Fchar(() => '4');
            Fchar(() => '5');
        }

        [Fact]
        public void Floats()
        {
            Ffloat(() => 3.14f);
            Ffloat(() => -3.14f);
            Ffloat(() => -9.75f);
            Ffloat(() => 9.75f);
            Ffloat(() => 0.141234f);
            Ffloat(() => 1233.11f);
        }

        [Fact]
        public void Doubles()
        {
            Fdouble(() => 3.14d);
            Fdouble(() => -3.14d);
            Fdouble(() => -9.75d);
            Fdouble(() => 9.75d);
            Fdouble(() => 0.141234d);
            Fdouble(() => 1233.11d);
        }
    }
}
