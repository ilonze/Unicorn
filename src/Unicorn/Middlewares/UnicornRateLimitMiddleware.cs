using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Managers.Caches;
using Unicorn.Options;
using Unicorn.Providers.RateLimits;

namespace Unicorn.Middlewares
{
    public class UnicornRateLimitMiddleware : UnicornMiddlewareBase<RateLimitOptions>
    {
        protected IUnicornCacheManager CacheManager { get; }
        public UnicornRateLimitMiddleware(
            UnicornContext context,
            IOptions<UnicornOptions> unicornOptions,
            IUnicornCacheManager cacheManager)
            : base(context.RouteRule.RateLimitOptions, context, unicornOptions)
        {
            CacheManager = cacheManager;
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true
                || UnicornOptions.RateLimitProviders?.ContainsKey(Options.Provider) != true)
            {
                await next(context);
                return;
            }
            var aggregateType = UnicornOptions.RateLimitProviders[Options.Provider];
            var provider = Services.GetRequiredService(aggregateType) as IRateLimitProvider;
            var featureKey = await provider.CreateKeyAsync(UnicornContext, context.RequestAborted);
            var isAllow = await CacheManager.SetRateLimitDataAsync(featureKey, TimeSpan.FromSeconds(Options.Period), Options.Limit);
            if (isAllow || Options?.IPAllowedList?.Contains(UnicornContext.RequestData.ClientIp) == true
                || Options?.UserAgentAllowedList?.Contains(UnicornContext.RequestData.Headers["User-Agent"]) == true)
            {
                await next(context);
                return;
            }
            UnicornContext.ResponseData = new ResponseData
            {
                StatusCode = Options.QuotaExceededStatusCode,
                StatusMessage = Options.QuotaExceededMessage,
            };
        }
    }
}
