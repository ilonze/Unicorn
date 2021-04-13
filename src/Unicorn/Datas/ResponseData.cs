using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Datas
{
    public class ResponseData
    {
        public byte[] Body { get; set; }
        public string BodyString { get; set; }
        public Dictionary<string, StringValues> Headers { get; set; }
        public int StatusCode { get; set; } = 200;
        public string StatusMessage { get; set; } = "OK";
        public string ContentType { get; set; }
        public List<(string name, string value, CookieOptions options)> AppendCookies { get; set; }
            = new List<(string name, string value, CookieOptions options)>();
        public List<(string name, CookieOptions options)> DeleteCookies { get; set; }
            = new List<(string name, CookieOptions options)>();

        public ResponseData AppendCookie(string name, string value, CookieOptions options = null)
        {
            AppendCookies.Add((name, value, options));
            return this;
        }

        public ResponseData DeleteCookie(string name, CookieOptions options = null)
        {
            DeleteCookies.Add((name, options));
            return this;
        }
    }
}
