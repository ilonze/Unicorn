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
    /// <summary>
    /// https://www.cnblogs.com/zhesong/articles/csrfanti.html
    /// http://www.voidcn.com/article/p-rmodpcfw-bve.html
    /// </summary>
    public class UnicornAntiForgeryMiddleware: UnicornMiddlewareBase<AntiForgeryOptions>
    {
        public UnicornAntiForgeryMiddleware(UnicornContext unicornContext, IOptions<UnicornOptions> options): base(unicornContext.RouteRule.AntiForgeryOptions, unicornContext, options)
        {

        }

        public override Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
