using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class LoadBalanceData
    {
        public long UseTimes { get; set; }
        public DateTimeOffset LastUseTime { get; set; }
        public int ConnectNumber { get; set; }
    }
}
