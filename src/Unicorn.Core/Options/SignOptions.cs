using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class SignOptions : IOptions
    {
        public bool IsEnabled { get; set; } = false;
        public string RequestProviderName { get; set; }
        public string ResponseProviderName { get; set; }
        public string RequestHeader { get; set; } = "X-Sign";
        public string ResponseHeader { get; set; } = "X-Sign";
        public int InvalidStatusCode { get; set; } = 400;
        public string InvalidMessage { get; set; }
        public string PrefixSalt { get; set; }
        public string PostfixSalt { get; set; }
        public string SignKey { get; set; }
        public string VerifyKey { get; set; }
        public Dictionary<string, string> Extra { get; set; }
    }
}
