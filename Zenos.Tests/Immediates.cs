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
            Test.AreEqual(0, () => 0);
            Test.AreEqual(1, () => 1);
            Test.AreEqual(-1, () => -1);
            Test.AreEqual(10, () => 10);
            Test.AreEqual(-10, () => -10);
            Test.AreEqual(2736, () => 2736);
            Test.AreEqual(-2736, () => -2736);
            Test.AreEqual(536870911, () => 536870911);
            Test.AreEqual(-536870912, () => -536870912);
        }

        [Test]
        public void Longs()
        {
            Test.AreEqual(-5368709121234L, () => -5368709121234L);
            Test.AreEqual(429496121113456735L, () => 429496121113456735L);
        }

        [Test]
        public void Boolean()
        {
            Test.AreEqual(true, () => true);
            Test.AreEqual(false, () => false);
        }


        [Test]
        public void Characters()
        {
            Test.AreEqual('a', () => 'a');
            Test.AreEqual('b', () => 'b');
            Test.AreEqual('c', () => 'c');
            Test.AreEqual('d', () => 'd');
            Test.AreEqual('e', () => 'e');
            Test.AreEqual('f', () => 'f');

            Test.AreEqual('0', () => '0');
            Test.AreEqual('1', () => '1');
            Test.AreEqual('2', () => '2');
            Test.AreEqual('3', () => '3');
            Test.AreEqual('4', () => '4');
            Test.AreEqual('5', () => '5');
        }

        [Test]
        public void Floats()
        {
            Test.AreEqual(3.140f, () => 3.14f);
            Test.AreEqual(-3.140f, () => -3.14f);
            Test.AreEqual(-9.750f, () => -9.75f);
            Test.AreEqual(9.750f, () => 9.75f);
            Test.AreEqual(0.141f, () => 0.141234f);
            Test.AreEqual(1233.114f, () => 1233.114f);
        }

        [Test]
        public void Doubles()
        {
            Test.AreEqual(3.140d, () => 3.14d);
            Test.AreEqual(-3.140d, () => -3.14d);
            Test.AreEqual(-9.750d, () => -9.75d);
            Test.AreEqual(9.750d, () => 9.75d);
            Test.AreEqual(0.141d, () => 0.141234d);
            Test.AreEqual(1233.114d, () => 1233.114d);
        }
    }
}
