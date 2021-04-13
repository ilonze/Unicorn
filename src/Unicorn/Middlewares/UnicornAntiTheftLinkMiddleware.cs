using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornAntiTheftLinkMiddleware : UnicornMiddlewareBase<AntiTheftLinkOptions>
    {
        public UnicornAntiTheftLinkMiddleware(UnicornContext unicornContext, IOptions<UnicornOptions> options) : base(unicornContext.RouteRule.AntiTheftLinkOptions, unicornContext, options)
        {

        }

        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var referer = context.Request.Headers["Referer"];
            if (Options?.IsEnabled != true || string.IsNullOrWhiteSpace(referer))
            {
                await next(context);
                return;
            }
            var refererUri = new Uri(referer);
            if (refererUri.Host == context.Request.Host.Value
                || Options.AllowHosts.Contains(refererUri.Host))
            {
                await next(context);
                return;
            }
            UnicornContext.ResponseData = new ResponseData
            {
                StatusCode = Options.BlockedStatusCode,
                StatusMessage = Options.BlockedMessage
            };
        }
    }
}
