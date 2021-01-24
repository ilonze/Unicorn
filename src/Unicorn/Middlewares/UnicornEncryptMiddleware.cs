using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;
using Unicorn.Providers.Encrypts;

namespace Unicorn.Middlewares
{
    public class UnicornEncryptMiddleware : UnicornMiddlewareBase<EncryptOptions>
    {
        public UnicornEncryptMiddleware(UnicornContext context, IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.EncryptOptions, context, unicornOptions)
        {

        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true)
            {
                await next(context);
                return;
            }
            if (!(Options?.RequestProvider).IsNullOrWhiteSpace()
                && UnicornOptions.SignProviders?.ContainsKey(Options.RequestProvider) == true)
            {
                if (!await DecryptAsync(context.RequestAborted))
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
            if (!(Options?.ResponseProvider).IsNullOrWhiteSpace()
                && UnicornOptions.SignProviders?.ContainsKey(Options.ResponseProvider) == true)
            {
                await EncryptAsync(context.RequestAborted);
            }
        }

        public async Task EncryptAsync(CancellationToken token = default)
        {
            var providerType = UnicornOptions.SignProviders[Options.ResponseProvider];
            var provider = Services.GetRequiredService(providerType) as IEncryptProvider;
            await provider.EncryptAsync(UnicornContext, Options, token);
        }

        public async Task<bool> DecryptAsync(CancellationToken token = default)
        {
            var providerType = UnicornOptions.SignProviders[Options.RequestProvider];
            var provider = Services.GetRequiredService(providerType) as IEncryptProvider;
            return await provider.DecryptAsync(UnicornContext, Options, token);
        }
    }
}
