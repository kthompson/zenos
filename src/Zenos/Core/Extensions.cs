using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Zenos.Core
{
    public static class Extensions
    {

        public static void AddRange(this List<byte> list, params int[] items)
        {
            list.AddRange(items.Select(b => (byte)b));
        }

        public static void AddRange<T>(this List<T> list, params T[] items)
        {
            list.AddRange(items);
        }

        public static string ToFileName(this string t)
        {
            return t.Replace('<', '_').Replace('>', '_');
        }

        public static string AppendRandom(this string t, int length = 32, string suffix = "")
        {
            return t.AppendRandom(string.Empty, length, suffix);
        }

        public static string AppendRandom(this string t, string prefix, int length = 32, string suffix = "")
        {
            var sb = new StringBuilder();
            sb.Append(t + prefix);

            while (sb.Length < length)
                sb.Append( Guid.NewGuid().ToString().Replace("-", "").ToLower());
            
            return sb.ToString().Substring(0, length) + suffix;
        }

        public static string Append(this string t, params string[] arguments)
        {
            var sb = new StringBuilder(t);
            foreach (var argument in arguments)
                sb.Append(argument);

            return sb.ToString();
        }
    }
}
