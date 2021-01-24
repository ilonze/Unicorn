using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Datas
{
    public class RequestData
    {
        public Dictionary<string, StringValues> Query { get; set; }
        public bool HasFormContentType { get; set; }
        public bool HasFormDataContentType { get; set; }
        public bool HasJsonContentType { get; set; }
        public bool HasTextContentType { get; set; }
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public Dictionary<string, StringValues> Headers { get; set; }
        public string Protocol { get; set; }
        public PathString Path { get; set; }
        public PathString PathBase { get; set; }
        public HostString Host { get; set; }
        public bool IsHttps { get; set; }
        public string Scheme { get; set; }
        public string Method { get; set; }
        public byte[] Body { get; set; }
        public Dictionary<string, StringValues> Form { get; set; }
        public IFormFileCollection Files { get; set; }
        public string Json { get; set; }
        public string Text { get; set; }
        public string Origin { get; set; }
        public string Url { get; set; }
        public string ClientIp { get; set; }
    }
}
