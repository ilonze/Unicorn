using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Datas
{
    public class RequestResponseDataBase
    {
        public byte[] Body { get; set; }
        public string BodyString { get; set; }
        public Dictionary<string, StringValues> Headers { get; set; }
        public string ContentType { get; set; }
    }
}
