using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public abstract class UnicornMiddlewareBase<TOptions> : IMiddleware
        where TOptions : IOptions
    {
        protected TOptions Options { get; }
        protected UnicornOptions UnicornOptions { get; }
        protected UnicornContext UnicornContext { get; }
        protected IServiceProvider Services { get; }
        protected UnicornMiddlewareBase(TOptions options, UnicornContext context, IOptions<UnicornOptions> unicornOptions)
        {
            Options = options;
            UnicornContext = context;
            UnicornOptions = unicornOptions.Value;
            Services = context.HttpContext.RequestServices;
        }
        public abstract Task InvokeAsync(HttpContext context, RequestDelegate next);
    }
}
