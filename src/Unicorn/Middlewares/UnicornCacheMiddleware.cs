using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Extensions;
using Unicorn.Managers.Caches;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornCacheMiddleware : UnicornMiddlewareBase<CacheOptions>
    {
        protected IUnicornCacheManager UnicornCacheManager { get; }
        public UnicornCacheMiddleware(
            UnicornContext context,
            IOptions<UnicornOptions> unicornOptions,
            IUnicornCacheManager unicornCacheManager)
            : base(context.RouteRule.CacheOptions, context, unicornOptions)
        {
            UnicornCacheManager = unicornCacheManager;
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "GET")
            {
                if (Options?.IsEnabled == true)
                {
                    var key = context.GetRequestUrl().Hash();
                    var responseData = await UnicornCacheManager.GetResponseDataAsync(key);
                    if (responseData != null)
                    {
                        UnicornContext.ResponseData = responseData;
                        return;
                    }
                    else
                    {
                        await next(context);
                        if (UnicornContext.ResponseData?.StatusCode == 200)
                        {
                            var seconds = Options.Expiry;
                            await UnicornCacheManager.SetResponseDataAsync(
                                key,
                                UnicornContext.ResponseData,
                                TimeSpan.FromSeconds(seconds));
                        }
                    }
                }
                else
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}
