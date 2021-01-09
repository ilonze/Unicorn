using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public abstract class UnicornMiddlewareBase<TOptions> : IMiddleware
        where TOptions : IOptions
    {
        protected TOptions Options { get; }
        protected UnicornContext UnicornContext { get; }
        protected UnicornMiddlewareBase(TOptions options, UnicornContext context)
        {
            Options = options;
            UnicornContext = context;
        }
        public abstract Task InvokeAsync(HttpContext context, RequestDelegate next);
    }
}
