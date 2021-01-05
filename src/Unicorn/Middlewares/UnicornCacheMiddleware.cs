using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Caches;
using Unicorn.Caching;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornCacheMiddleware : IMiddleware
    {
        protected UnicornOptions Options { get; }
        protected UnicornContext UnicornContext { get; }
        protected IUnicornCacheManager UnicornCacheManager { get; }
        public UnicornCacheMiddleware(
            IOptions<UnicornOptions> options,
            UnicornContext unicornContext)
        {
            Options = options.Value;
            UnicornContext = unicornContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "GET")
            {
                var globalOptions = Options.GlobalOptions;
                var routeRule = UnicornContext.RouteRule;
                if (routeRule.CacheOptions?.IsCahce == true
                    || globalOptions.CacheOptions?.IsCahce == true)
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
                            var seconds = routeRule.CacheOptions == null
                                || routeRule.CacheOptions?.TtlSeconds == 0
                                ? globalOptions.CacheOptions.TtlSeconds
                                : routeRule.CacheOptions.TtlSeconds;
                            await UnicornCacheManager.SetResponseDataAsync(key.Md5(), UnicornContext.ResponseData, seconds);
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
