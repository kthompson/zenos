using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Zenos.Tests
{
    public class ImmediateTests
    {
        [Fact]
        public void Integers()
        {
            var func = Test.Runs<Int32Delegate, int>(f => f());

            func(() => 0);
            func(() => 1);
            func(() => -1);
            func(() => 10);
            func(() => -10);
            func(() => 2736);
            func(() => -2736);
            func(() => 536870911);
            func(() => -536870912);
        }

        [Fact]
        public void Longs()
        {
            var func = Test.Runs<Int64Delegate, long>(f => f());

            func(() => 0x1234567891011120L);
            func(() => -5368709121234L);
            func(() => 429496121113456735L);
        }

        [Fact]
        public void Boolean()
        {
            var func = Test.Runs<BoolDelegate, bool>(f => f());

            func(() => true);
            func(() => false);
        }

        [Fact]
        public void Characters()
        {
            var func = Test.Runs<CharDelegate, char>(f => f());

            func(() => 'a');
            func(() => 'b');
            func(() => 'c');
            func(() => 'd');
            func(() => 'e');
            func(() => 'f');

            func(() => '0');
            func(() => '1');
            func(() => '2');
            func(() => '3');
            func(() => '4');
            func(() => '5');
        }

        [Fact]
        public void Floats()
        {
            var func = Test.Runs<SingleDelegate, float>(f => f());

            func(() => 3.14f);
            func(() => -3.14f);
            func(() => -9.75f);
            func(() => 9.75f);
            func(() => 0.141234f);
            func(() => 1233.11f);
        }

        [Fact]
        public void Doubles()
        {
            var func = Test.Runs<DoubleDelegate, double>(f => f());

            func(() => 3.14d);
            func(() => -3.14d);
            func(() => -9.75d);
            func(() => 9.75d);
            func(() => 0.141234d);
            func(() => 1233.11d);
        }
    }
}
