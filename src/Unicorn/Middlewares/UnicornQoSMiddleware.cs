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
    public class UnicornQoSMiddleware : UnicornMiddlewareBase<QoSOptions>
    {
        public UnicornQoSMiddleware(UnicornContext context, IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.QoSOptions, context, unicornOptions)
        {

        }
        public override Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
