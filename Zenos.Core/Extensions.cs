using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Zenos.Core
{
    public static class Extensions
    {
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
            var tempString = t + prefix;

            while (tempString.Length < length)
                tempString += Guid.NewGuid().ToString().Replace("-", "").ToLower();

            return tempString.Substring(0, length) + suffix;
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
