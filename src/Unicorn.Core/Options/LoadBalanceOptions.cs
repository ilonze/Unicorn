using System;
using System.Collections.Generic;
using System.Text;

namespace Unicorn.Options
{
    public class LoadBalanceOptions
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public int Expiry { get; set; }
    }
}
