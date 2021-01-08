using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class BytesExtensions
    {
        public static string Md5(this byte[] bytes)
        {
            string pwd = "";
            var md5 = MD5.Create();
            byte[] s = md5.ComputeHash(bytes);
            for (int i = 0; i < s.Length; i++)
            {
                pwd += s[i].ToString("X");
            }
            return pwd;
        }
    }
}
