using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornSignMiddleware : UnicornMiddlewareBase<SignOptions>
    {
        public UnicornSignMiddleware(UnicornContext context)
            : base(context.RouteRule.SignOptions, context)
        {

        }
        public override Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
