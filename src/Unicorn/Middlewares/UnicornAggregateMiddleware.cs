using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Managers.Aggregates;
using Unicorn.Options;

namespace Unicorn.Middlewares
{
    public class UnicornAggregateMiddleware : UnicornMiddlewareBase<AggregateOptions>
    {
        protected IUnicornAggregateManager AggregateManager { get; }
        public UnicornAggregateMiddleware(
            UnicornContext context,
            IUnicornAggregateManager aggregateManager)
            : base(context.RouteRule.AggregateOptions, context)
        {
            AggregateManager = aggregateManager;
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options?.IsEnabled == true)
            {
                await next(context);
                return;
            }

            var responseData = await AggregateManager.AggregateAsync(context, UnicornContext);
            UnicornContext.ResponseData = responseData;
        }
    }
}
