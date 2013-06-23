using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zenos.Tests
{
    static class Data
    {
        public static IEnumerable<Int32> GetInt32s()
        {
            yield return int.MaxValue;
            yield return int.MinValue;
            yield return 0;
            yield return 123654;
            yield return 1236234674;
            yield return 999999999;
            yield return 50;
            yield return 27;
        }

        public static IEnumerable<Int16> GetInt16s()
        {
            yield return short.MaxValue;
            yield return short.MinValue;
            yield return 0;
            yield return 12365;
            yield return 12374;
            yield return 9999;
            yield return 50;
            yield return 27;
        }

        public static IEnumerable<byte> GetBytes()
        {
            yield return byte.MinValue;
            yield return 50;
            yield return 59;
            yield return 90;
            yield return byte.MaxValue;
        }


        public static IEnumerable<char> GetChars()
        {
            yield return char.MaxValue;
            yield return char.MinValue;
            yield return 'a';
            yield return '0';
            yield return 'z';
            yield return 'A';
        }

        public static IEnumerable<Single> GetSingles()
        {
            yield return Single.MaxValue;
            yield return Single.MinValue;
            yield return 0.5f;
            yield return 12365;
            yield return 123.74f;
            yield return 9999;
            yield return 50.1321654f;
            yield return 2720938902384902834.034234234234323423f;
        }

        public static IEnumerable<Double> GetDoubles()
        {
            yield return Double.MaxValue;
            yield return Double.MinValue;
            yield return 0.5D;
            yield return 12365D;
            yield return 123.74D;
            yield return 9999D;
            yield return 50.1321654D;
            yield return 2720938902384902834.034234234234323422334234234234234234234230989080890812321313D;
        }
    }
}
