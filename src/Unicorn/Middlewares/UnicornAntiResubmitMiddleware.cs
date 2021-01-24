using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Managers.Caches;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornAntiResubmitMiddleware : UnicornMiddlewareBase<AntiResubmitOptions>
    {
        protected IUnicornCacheManager UnicornCacheManager { get; }
        public UnicornAntiResubmitMiddleware(
            UnicornContext context,
            IOptions<UnicornOptions> unicornOptions,
            IUnicornCacheManager unicornCacheManager)
            : base(context.RouteRule.AntiResubmitOptions, context, unicornOptions)
        {
            UnicornCacheManager = unicornCacheManager;
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true)
            {
                await next(context);
                return;
            }
            var ipBytes = Encoding.UTF8.GetBytes(UnicornContext.RequestData.ClientIp);
            var pathBytes = Encoding.UTF8.GetBytes(UnicornContext.RequestData.Url);
            var bodyBytes = UnicornContext.RequestData.Body;
            var key = ipBytes.Union(pathBytes).Union(bodyBytes).ToArray().Hash();
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
