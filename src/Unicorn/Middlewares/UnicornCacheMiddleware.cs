using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Unicorn.Caches;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornCacheMiddleware : IMiddleware
    {
        protected CacheOptions Options { get; }
        protected UnicornContext UnicornContext { get; }
        protected IUnicornCacheManager UnicornCacheManager { get; }
        public UnicornCacheMiddleware(UnicornContext unicornContext)
        {
            Options = unicornContext.RouteRule.CacheOptions;
            UnicornContext = unicornContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "GET")
            {
                if (Options?.IsEnabled == true)
                {
                    var key = context.Request.Scheme + "://" + context.Request.Host.Value + context.Request.Path.Value + context.Request.QueryString.Value;
                    var responseData = await UnicornCacheManager.GetResponseDataAsync(key.Md5());
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
                            var seconds = Options.TtlSeconds;
                            await UnicornCacheManager.SetResponseDataAsync(key.Md5(), UnicornContext.ResponseData, TimeSpan.FromSeconds(seconds));
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
