using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornEncryptMiddleware : UnicornMiddlewareBase<EncryptOptions>
    {
        public UnicornEncryptMiddleware(UnicornContext context)
            : base(context.RouteRule.EncryptOptions, context)
        {

        }
        public override Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
