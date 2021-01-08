using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Unicorn
{
    public class RequestData
    {
        public string Method { get; set; }
        public Dictionary<string, StringValues> Headers { get; set; } = new Dictionary<string, StringValues>();
        public byte[] Body { get; set; }
        public string BodyString { get; set; }
    }
}
