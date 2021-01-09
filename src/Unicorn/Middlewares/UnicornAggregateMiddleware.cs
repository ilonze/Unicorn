using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornAggregateMiddleware : UnicornMiddlewareBase<AggregateOptions>
    {
        public UnicornAggregateMiddleware(UnicornContext context)
            : base(context.RouteRule.AggregateOptions, context)
        {
        }
        public override Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
