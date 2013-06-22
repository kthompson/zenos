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
            Test.Runs(() => 0);
            Test.Runs(() => 1);
            Test.Runs(() => -1);
            Test.Runs(() => 10);
            Test.Runs(() => -10);
            Test.Runs(() => 2736);
            Test.Runs(() => -2736);
            Test.Runs(() => 536870911);
            Test.Runs(() => -536870912);
        }

        [Test]
        public void Longs()
        {
            Test.Runs(() => 0x1234567891011120L);
            Test.Runs(() => -5368709121234L);
            Test.Runs(() => 429496121113456735L);
        }

        [Test]
        public void Boolean()
        {
            Test.Runs(() => true);
            Test.Runs(() => false);
        }


        [Test]
        public void Characters()
        {
            Test.Runs(() => 'a');
            Test.Runs(() => 'b');
            Test.Runs(() => 'c');
            Test.Runs(() => 'd');
            Test.Runs(() => 'e');
            Test.Runs(() => 'f');

            Test.Runs(() => '0');
            Test.Runs(() => '1');
            Test.Runs(() => '2');
            Test.Runs(() => '3');
            Test.Runs(() => '4');
            Test.Runs(() => '5');
        }

        [Test]
        public void Floats()
        {
            Test.Runs(() => 3.14f);
            Test.Runs(() => -3.14f);
            Test.Runs(() => -9.75f);
            Test.Runs(() => 9.75f);
            Test.Runs(() => 0.141234f);
            Test.Runs(() => 1233.11f);
        }

        [Test]
        public void Doubles()
        {
            Test.Runs(() => 3.14d);
            Test.Runs(() => -3.14d);
            Test.Runs(() => -9.75d);
            Test.Runs(() => 9.75d);
            Test.Runs(() => 0.141234d);
            Test.Runs(() => 1233.11d);
        }
    }
}
