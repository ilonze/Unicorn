using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.RateLimits
{
    [ProviderName(ProviderName)]
    public class UserAgentRateLimitProvider : IRateLimitProvider
    {
        public const string ProviderName = "UserAgent";
        public string Name => ProviderName;

        public Task<string> CreateKeyAsync(UnicornContext context, CancellationToken token = default)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromResult("");
            }
            var featureKey = context.RequestData.Url + "@" + context.RequestData.Headers["User-Agent"];
            featureKey = featureKey.Md5();
            return Task.FromResult(featureKey);
        }
    }
}
