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
using Unicorn.Providers.LoadBalances;

namespace Unicorn.Middlewares
{
    public class UnicornLoadBalanceMiddleware : UnicornMiddlewareBase<LoadBalanceOptions>
    {
        protected IUnicornCacheManager CacheManager { get; set; }
        public UnicornLoadBalanceMiddleware(
            UnicornContext context,
            IUnicornCacheManager cacheManager,
            IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.LoadBalancerOptions, context, unicornOptions)
        {
            CacheManager = cacheManager;
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true
                || (Options?.Provider).IsNullOrWhiteSpace())
            {
                await next(context);
                UnicornContext.Services = UnicornContext.AlternativeServices.ToDictionary(r => r.Key, r => r.Value.FirstOrDefault());
                return;
            }

            var providerType = UnicornOptions.LoadBalanceProviders[Options.Provider];
            var provider = context.RequestServices.GetRequiredService(providerType) as ILoadBalanceProvider;

            var services = UnicornContext.DownstreamRoutes.ToDictionary(
                r => r.DownstreamServiceName,
                r => UnicornContext.AlternativeServices[r.DownstreamServiceName]);

            UnicornContext.Services ??= new Dictionary<string, Service>();
            foreach (var dsRoute in UnicornContext.DownstreamRoutes)
            {
                var datas = await CacheManager.GetServicesLoadBalaceDataAsync(
                    dsRoute.DownstreamServiceName,
                    services[dsRoute.DownstreamServiceName].Select(r => r.Id), context.RequestAborted);

                var service = await provider.ExecuteAsync(
                    services[dsRoute.DownstreamServiceName],
                    datas,
                    context.RequestAborted);

                UnicornContext.Services[dsRoute.DownstreamServiceName] = service;

                var data = datas[dsRoute.DownstreamServiceName];

                data.ConnectNumber++;
                data.LastUseTime = DateTimeOffset.Now;
                data.UseTimes++;

                await CacheManager.SetServiceLoadBalaceDataAsync(
                    service.Name,
                    service.Id,
                    data,
                    context.RequestAborted);
            }

            await next(context);
        }
    }
}
