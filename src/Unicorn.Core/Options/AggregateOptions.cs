using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class AggregateOptions
    {
        public string AggregateProvider { get; set; }
        public Dictionary<string, string> AggregateKeys { get; set; } = new Dictionary<string, string>();
        public bool AllowPartialResponse { get; set; } = true;
    }
}
