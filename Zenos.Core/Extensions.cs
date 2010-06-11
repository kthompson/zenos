using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Zenos.Core
{
    public static class Extensions
    {
        public static string AppendRandom(this string prefix, int length = 32, string suffix = "")
        {
            var tempString = prefix;

            while (tempString.Length < length)
                tempString += Guid.NewGuid().ToString().Replace("-", "").ToLower();

            return tempString.Substring(0, length) + suffix;
        }

        public static string Append(this string prefix, params string[] arguments)
        {
            var sb = new StringBuilder(prefix);
            foreach (var argument in arguments)
                sb.Append(argument);

            return sb.ToString();
        }
    }
}
