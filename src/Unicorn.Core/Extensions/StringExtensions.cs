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
    }
}
