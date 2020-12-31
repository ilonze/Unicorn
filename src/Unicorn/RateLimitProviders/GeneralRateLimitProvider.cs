using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.RateLimitProviders
{
    public class GeneralRateLimitProvider : IRateLimitProvider
    {
        public const string ProviderName = "General";
        public string Name => ProviderName;
    }
}
