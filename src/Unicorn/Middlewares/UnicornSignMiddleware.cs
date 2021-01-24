using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;
using Unicorn.Providers.Signs;

namespace Unicorn.Middlewares
{
    public class UnicornSignMiddleware : UnicornMiddlewareBase<SignOptions>
    {
        public UnicornSignMiddleware(UnicornContext context, IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.SignOptions, context, unicornOptions)
        {

        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true)
            {
                await next(context);
                return;
            }
            if (!(Options?.RequestProviderName).IsNullOrWhiteSpace()
                && UnicornOptions.SignProviders?.ContainsKey(Options.RequestProviderName) == true)
            {
                if(!await VerifyAsync(context.RequestAborted))
                {
                    UnicornContext.ResponseData = new ResponseData
                    {
                        StatusCode = Options.InvalidStatusCode,
                        StatusMessage = Options.InvalidMessage
                    };
                    return;
                }
            }
            await next(context);
            if (!(Options?.ResponseProviderName).IsNullOrWhiteSpace()
                && UnicornOptions.SignProviders?.ContainsKey(Options.ResponseProviderName) == true)
            {
                await SignAsync(context.RequestAborted);
            }
        }

        public async Task SignAsync(CancellationToken token = default)
        {
            var providerType = UnicornOptions.SignProviders[Options.ResponseProviderName];
            var provider = Services.GetRequiredService(providerType) as ISignProvider;
            await provider.SignAsync(UnicornContext, Options, token);
        }

        public async Task<bool> VerifyAsync(CancellationToken token = default)
        {
            var providerType = UnicornOptions.SignProviders[Options.RequestProviderName];
            var provider = Services.GetRequiredService(providerType) as ISignProvider;
            return await provider.VerifyAsync(UnicornContext, Options, token);
        }
    }
}
