using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Caches
{
    public class ResponseData
    {
        public byte[] Data { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public int StatusCode { get; set; } = 200;
    }
}
