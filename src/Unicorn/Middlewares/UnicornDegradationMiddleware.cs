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
    public class UnicornDegradationMiddleware : UnicornMiddlewareBase<UnicornOptions>
    {
        public UnicornDegradationMiddleware(UnicornContext context, IOptions<UnicornOptions> options)
            :base(options.Value, context)
        {

        }
        public override Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
