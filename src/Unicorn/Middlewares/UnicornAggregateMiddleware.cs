using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornAggregateMiddleware : UnicornMiddlewareBase<AggregateOptions>
    {
        public UnicornAggregateMiddleware(UnicornContext context)
            : base(context.RouteRule.AggregateOptions, context)
        {
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled == true)
            {
                await next(context);
                return;
            }


        }
    }
}
