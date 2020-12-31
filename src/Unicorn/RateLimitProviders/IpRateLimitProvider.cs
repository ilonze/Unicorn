using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.RateLimitProviders
{
    public class IpRateLimitProvider : IRateLimitProvider
    {
        public const string ProviderName = "IP";
        public string Name => ProviderName;
    }
}
