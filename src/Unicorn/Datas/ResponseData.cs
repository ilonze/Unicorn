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
        public Dictionary<string, StringValues> Headers { get; set; } = new Dictionary<string, StringValues>();
        public int StatusCode { get; set; } = 200;
        public string StatusMessage { get; set; } = "OK";
    }
}
