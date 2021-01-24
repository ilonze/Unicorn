using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;
using Unicorn.Providers.Aggregates;

namespace Unicorn.Middlewares
{
    public class UnicornAggregateMiddleware : UnicornMiddlewareBase<AggregateOptions>
    {
        public UnicornAggregateMiddleware(
            UnicornContext context,
            IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.AggregateOptions, context, unicornOptions)
        {
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
            if (Options?.IsEnabled != true
                || (Options?.Provider).IsNullOrWhiteSpace())
            {
                return;
            }
            if (UnicornOptions.AggregateProviders?.ContainsKey(Options.Provider) != true)
            {
                return;
            }
            var aggregateType = UnicornOptions.AggregateProviders[Options.Provider];
            var provider = Services.GetRequiredService(aggregateType) as IAggregateProvider;
            var responseData = await provider.AggregateAsync(UnicornContext, Options, context.RequestAborted);
            UnicornContext.ResponseData = responseData;
        }
    }
}
