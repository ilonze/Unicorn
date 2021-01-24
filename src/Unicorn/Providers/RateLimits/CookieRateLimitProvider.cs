﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Providers.RateLimits
{
    [ProviderName(ProviderName)]
    public class CookieRateLimitProvider : IRateLimitProvider
    {
        public const string ProviderName = "Cookie";
        public string Name => ProviderName;

        public Task<string> CreateKeyAsync(UnicornContext context, CancellationToken token = default)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromResult("");
            }
            var featureKey = context.RequestData.Url + "@" + context.RequestData.Cookies.OrderBy(r => r.Key).Select(r => r.Key + "=" + r.Value).JoinAsString("&");
            featureKey = featureKey.Hash();
            return Task.FromResult(featureKey);
        }
    }
}
