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
    public class UnicornAuthenticationMiddleware : UnicornMiddlewareBase<AuthenticationOptions>
    {
        public UnicornAuthenticationMiddleware(UnicornContext context, IOptions<UnicornOptions> unicornOptions)
            : base(context.RouteRule.AuthenticationOptions, context, unicornOptions)
        {

        }
        public override Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
