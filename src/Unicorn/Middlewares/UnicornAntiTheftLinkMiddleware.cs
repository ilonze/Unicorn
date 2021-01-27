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

        public override Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
