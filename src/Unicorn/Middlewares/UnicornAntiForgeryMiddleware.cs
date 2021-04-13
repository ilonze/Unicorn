using Microsoft.AspNetCore.Antiforgery;
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
        protected Lazy<IAntiforgery> Antiforgery { get; }
        public UnicornAntiForgeryMiddleware(
            UnicornContext unicornContext,
            IOptions<UnicornOptions> options,
            Lazy<IAntiforgery> antiforgery)
            : base(unicornContext.RouteRule.AntiForgeryOptions, unicornContext, options)
        {
            Antiforgery = antiforgery;
        }

        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled != true)
            {
                await next(context);
                return;
            }

            if (context.Request.Method == "GET")
            {
                var tokens = Antiforgery.Value.GetAndStoreTokens(context);
                UnicornContext.ResponseData ??= new ResponseData();
                UnicornContext.ResponseData.AppendCookie(Options?.CookieName ?? "X-XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
            }

            if (Options?.IsAutoValidate == true && (context.Request.Method == "GET" || context.Request.Method == "HEAD" || context.Request.Method == "TRACE" || context.Request.Method == "OPTIONS"))
            {
                await next(context);
                return;
            }

            try
            {
                await Antiforgery.Value.ValidateRequestAsync(context);
                await next(context);
            }
            catch (AntiforgeryValidationException)
            {
                UnicornContext.ResponseData ??= new ResponseData();
                UnicornContext.ResponseData.StatusCode = 400;
                UnicornContext.ResponseData.StatusMessage = "Antiforgery invalid";
            }
        }
    }
}
