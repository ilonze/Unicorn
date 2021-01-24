using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Managers.Caches;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornCorsMiddleware : UnicornMiddlewareBase<CorsOptions>
    {
        protected IUnicornCacheManager UnicornCacheManager { get; }
        public UnicornCorsMiddleware(
            UnicornContext context,
            IOptions<UnicornOptions> unicornOptions,
            IUnicornCacheManager unicornCacheManager)
            : base(context.RouteRule.CorsOptions, context, unicornOptions)
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
            else if (context.Request.Method != "OPTIONS")
            {
                await next(context);
            }
            UnicornContext.ResponseData ??= new ResponseData();
            SetCors(context.Request.Host.Value, UnicornContext.ResponseData);
        }

        private void SetCors(string host, ResponseData responseData)
        {
            if (Options.Origins.Contains(host) || Options.Origins.Contains("*"))
            {
                responseData.Headers["Access-Control-Allow-Origin"] = Options.Origins.Contains(host) ? host : "*";
                responseData.Headers["Access-Control-Allow-Headers"] = Options.Headers;
                responseData.Headers["Access-Control-Allow-Methods"] = Options.Methods;
                responseData.Headers["Access-Control-Allow-Credentials"] = Options.Credentials.ToString().ToLower();
            }
        }
    }
}
