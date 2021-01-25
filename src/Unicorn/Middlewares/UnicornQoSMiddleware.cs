using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Managers.Caches;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornQoSMiddleware : UnicornMiddlewareBase<QoSOptions>
    {
        protected IUnicornCacheManager CacheManager { get; }
        public UnicornQoSMiddleware(
            UnicornContext context,
            IOptions<UnicornOptions> unicornOptions,
            IUnicornCacheManager cacheManager)
            : base(context.RouteRule.QoSOptions, context, unicornOptions)
        {
            CacheManager = cacheManager;
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true)
            {
                await next(context);
                return;
            }

            foreach (var serviceName in UnicornContext.AlternativeServices.Keys.ToArray())
            {
                var ids = await CacheManager.FilterQoSAsync(serviceName, UnicornContext.AlternativeServices[serviceName].Select(r => r.Id), Options.Duration, context.RequestAborted);
                UnicornContext.AlternativeServices[serviceName] = UnicornContext.AlternativeServices[serviceName].Where(r => ids.Contains(r.Id));
            }
            await next(context);
            foreach (var exception in UnicornContext.Exceptions)
            {
                if (exception.Value != null)
                {
                    var serviceName = exception.Key;
                    await CacheManager.SaveQoSDataAsync(
                        serviceName,
                        new[] { UnicornContext.Services[serviceName].Id },
                        Options.Threshold,
                        Options.Timeout,
                        Options.Duration,
                        context.RequestAborted);
                }
            }
        }
    }
}
