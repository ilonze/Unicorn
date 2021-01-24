using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static string Md5(this string str)
        {
            return Encoding.UTF8.GetBytes(str).Md5();
        }

        public static string Hash(this string str)
        {
            return Encoding.UTF8.GetBytes(str).Hash();
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string IsInterned(this string str)
        {
            return string.IsInterned(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string Format(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string JoinAsString(this IEnumerable<string> strs, string splitor)
        {
            return string.Join(splitor, strs);
        }
    }
}
