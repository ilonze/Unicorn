using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;
using Unicorn.Providers.DataFormats;

namespace Unicorn.Middlewares
{
    public class UnicornDataFormatMiddleware : UnicornMiddlewareBase<DataFormatOptions>
    {
        public UnicornDataFormatMiddleware(UnicornContext context, IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.DataFormatOptions, context, unicornOptions)
        {

        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true)
            {
                await next(context);
                return;
            }
            if (!(Options?.RequestProvider).IsNullOrWhiteSpace())
            {
                await ConvertRequestDataAsync(context.RequestAborted);
            }
            await next(context);
            if (!(Options.ResponseProvider).IsNullOrWhiteSpace())
            {
                await ConvertResponseDataAsync(context.RequestAborted);
            }
        }

        private async Task ConvertRequestDataAsync(CancellationToken token = default)
        {
            if (UnicornOptions.DataFormatProviders?.ContainsKey(Options.RequestProvider) != true)
            {
                return;
            }
            var dataFormatType = UnicornOptions.DataFormatProviders[Options.RequestProvider];
            var provider = Services.GetRequiredService(dataFormatType) as IDataFormatProvider;
            UnicornContext.RequestData = await provider.ConvertAsync(UnicornContext, UnicornContext.RequestData, token);
        }

        private async Task ConvertResponseDataAsync(CancellationToken token = default)
        {
            if (UnicornOptions.DataFormatProviders?.ContainsKey(Options.ResponseProvider) != true)
            {
                return;
            }
            var dataFormatType = UnicornOptions.DataFormatProviders[Options.ResponseProvider];
            var provider = Services.GetRequiredService(dataFormatType) as IDataFormatProvider;
            UnicornContext.ResponseData = await provider.ConvertAsync(UnicornContext, UnicornContext.ResponseData, token);
        }
    }
}
