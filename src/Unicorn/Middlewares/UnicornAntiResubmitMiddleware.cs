using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Caches;
using Unicorn.Extensions;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornAntiResubmitMiddleware : IMiddleware
    {
        protected AntiResubmitOptions Options { get; }
        protected UnicornContext UnicornContext { get; }
        protected IUnicornCacheManager UnicornCacheManager { get; }
        public UnicornAntiResubmitMiddleware(
            UnicornContext unicornContext,
            IUnicornCacheManager unicornCacheManager)
        {
            Options = unicornContext.RouteRule.AntiResubmitOptions;
            UnicornContext = unicornContext;
            UnicornCacheManager = unicornCacheManager;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true)
            {
                await next(context);
                return;
            }
            var ipBytes = context.Connection.RemoteIpAddress.GetAddressBytes();
            var pathBytes = Encoding.UTF8.GetBytes(context.GetRequestUrl());
            var bodyBytes = UnicornContext.RequestData.Body;
            var key = ipBytes.Union(pathBytes).Union(bodyBytes).ToArray().Md5();
            var milliseconds = Options.Period;
            var result = await UnicornCacheManager.CheckAntiResubmitAsync(
                key,
                TimeSpan.FromMilliseconds(milliseconds));
            if (result)
            {
                await next(context);
                return;
            }
            else
            {
                UnicornContext.ResponseData = new ResponseData
                {
                    StatusCode = Options.BlockedStatusCode,
                    StatusMessage = Options.BlockedMessage
                };
                return;
            }
        }
    }
}
