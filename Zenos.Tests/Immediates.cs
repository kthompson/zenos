using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Zenos.Tests
{
    [TestFixture]
    public class ImmediateTests
    {
        [Test]
        public void Integers()
        {
            Test.Runs<Int32Delegate>(() => 0);
            Test.Runs<Int32Delegate>(() => 1);
            Test.Runs<Int32Delegate>(() => -1);
            Test.Runs<Int32Delegate>(() => 10);
            Test.Runs<Int32Delegate>(() => -10);
            Test.Runs<Int32Delegate>(() => 2736);
            Test.Runs<Int32Delegate>(() => -2736);
            Test.Runs<Int32Delegate>(() => 536870911);
            Test.Runs<Int32Delegate>(() => -536870912);
        }

        [Test]
        public void Longs()
        {
            Test.Runs<Int64Delegate>(() => 0x1234567891011120L);
            Test.Runs<Int64Delegate>(() => -5368709121234L);
            Test.Runs<Int64Delegate>(() => 429496121113456735L);
        }

        [Test]
        public void Boolean()
        {
            Test.Runs<BoolDelegate>(() => true);
            Test.Runs<BoolDelegate>(() => false);
        }


        [Test]
        public void Characters()
        {
            Test.Runs<CharDelegate>(() => 'a');
            Test.Runs<CharDelegate>(() => 'b');
            Test.Runs<CharDelegate>(() => 'c');
            Test.Runs<CharDelegate>(() => 'd');
            Test.Runs<CharDelegate>(() => 'e');
            Test.Runs<CharDelegate>(() => 'f');

            Test.Runs<CharDelegate>(() => '0');
            Test.Runs<CharDelegate>(() => '1');
            Test.Runs<CharDelegate>(() => '2');
            Test.Runs<CharDelegate>(() => '3');
            Test.Runs<CharDelegate>(() => '4');
            Test.Runs<CharDelegate>(() => '5');
        }

        [Test]
        public void Floats()
        {
            Test.Runs<SingleDelegate>(() => 3.14f);
            Test.Runs<SingleDelegate>(() => -3.14f);
            Test.Runs<SingleDelegate>(() => -9.75f);
            Test.Runs<SingleDelegate>(() => 9.75f);
            Test.Runs<SingleDelegate>(() => 0.141234f);
            Test.Runs<SingleDelegate>(() => 1233.11f);
        }

        [Test]
        public void Doubles()
        {
            Test.Runs<DoubleDelegate>(() => 3.14d);
            Test.Runs<DoubleDelegate>(() => -3.14d);
            Test.Runs<DoubleDelegate>(() => -9.75d);
            Test.Runs<DoubleDelegate>(() => 9.75d);
            Test.Runs<DoubleDelegate>(() => 0.141234d);
            Test.Runs<DoubleDelegate>(() => 1233.11d);
        }
    }
}
