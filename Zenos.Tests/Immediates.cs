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
            Fint(() => 0, context =>
            {
                var expected = new List<byte>
                {
                    0x55,                           // push   rbp
                    0x48, 0x89, 0xe5,               // mov    rbp,rsp
                    0x48, 0x83, 0xec, 0x10,         // sub    16, rsp

                    0x68, 0x00, 0x00, 0x00, 0x00,   // push   0x0
                    0x58,                           // pop    rax

                    0x48, 0x83, 0xc4, 0x10,         // add    16, rsp

                    0x5d,                           // pop    rbp
                    0xc3,                           // retq   
                    
                    0x66, 0x0F, 0x1F, 0x84, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                    // nop
                    0x0F, 0x1F, 0x00                // nop
                };

                var actual = context.Code;
                AssertSequenceDetailed(expected, actual);

                Assert.Equal(expected, actual);
            });
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
                    0x55,                                                       // push  rbp
                    0x48, 0x89, 0xe5,                                           // mov   rbp, rsp
                    0x48, 0x83, 0xec, 0x10,                                     // sub    16, rsp

                    0x48, 0xb8, 0x30, 0x46, 0x98, 0x3c, 0xd5, 0x62, 0x04, 0x00, // mov   rax, -1234567891011120
                    0x50,                                                       // push  rax
                    0x58,                                                       // pop   rax

                    
                    0x48, 0x83, 0xc4, 0x10,                                     // add    16, rsp
                    0x5d,                                                       // pop   rbp
                    0xc3,                                                       // ret  
                    0x66, 0x0F, 0x1F, 0x44, 0x00, 0x00                          // nop
                };

                var actual = context.Code;
                AssertSequenceDetailed(expected, actual);

                Assert.Equal(expected, actual);
            });
            Flong(() => -5368709121234L);
            Flong(() => 429496121113456735L);
        }

        private static void AssertSequenceDetailed(IReadOnlyList<byte> expected, IReadOnlyList<byte> actual)
        {
            if (actual.SequenceEqual(expected)) 
                return;

            var min = Math.Min(actual.Count, expected.Count);
            for (var i = 0; i < min; i++)
            {
                var diff = Equals(expected[i], actual[i]) ? "" : " !!!!";
                Trace.WriteLine(
                    string.Format("[{0,3}] - Expected: 0x{1}, Got: 0x{2}{3}", i, 
                    expected[i].ToString("x2"),
                    actual[i].ToString("x2"), 
                    diff));
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
