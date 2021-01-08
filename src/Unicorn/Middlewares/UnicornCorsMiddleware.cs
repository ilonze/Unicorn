using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Caches;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornCorsMiddleware : IMiddleware
    {
        protected CorsOptions Options { get; }
        protected UnicornContext UnicornContext { get; }
        protected IUnicornCacheManager UnicornCacheManager { get; }
        public UnicornCorsMiddleware(
            UnicornContext unicornContext,
            IUnicornCacheManager unicornCacheManager)
        {
            Options = unicornContext.RouteRule.CorsOptions;
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
                responseData.Headers["Access-Control-Allow-Origin"] = Options.Origins.Contains("*") ? "*" : host;
                responseData.Headers["Access-Control-Allow-Headers"] = string.Join(",", Options.Headers.ToArray());
                responseData.Headers["Access-Control-Allow-Methods"] = string.Join(",", Options.Methods.ToArray());
                responseData.Headers["Access-Control-Allow-Credentials"] = Options.Credentials.ToString().ToLower();
            }
        }
    }
}
