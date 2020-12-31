using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.RateLimitProviders
{
    public class UserAgentRateLimitProvider : IRateLimitProvider
    {
        public const string ProviderName = "UserAgent";
        public string Name => ProviderName;
    }
}
