using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unicorn.AggregateProviders;
using Unicorn.Handlers;
using Unicorn.Options;
using RouteData = Microsoft.AspNetCore.Routing.RouteValueDictionary;

namespace Unicorn.Middlewares
{
    public class UnicornMiddleware : UnicornMiddlewareBase<UnicornOptions>
    {
        protected IRouteHandler RouteHandler { get; }
        protected IServiceHandler _serviceHandler { get; }
        protected IAggregateProvider _aggregateProvider { get; }
        public UnicornMiddleware(
            IOptions<UnicornOptions> options,
            IRouteHandler routeHandler,
            UnicornContext context,
            IServiceHandler serviceHandler)
            : base(options.Value, context)
        {
            RouteHandler = routeHandler;
            _serviceHandler = serviceHandler;
        }
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //TODO:请求加签验签 Sign
            //TODO:服务降级升级 
            //TODO:请求加密解密 Encrypt
            //TODO:处理熔断 QoS
            //TODO:处理限流 Limit
            //TODO:黑白名单 AllowedList&BlockedList
            //TODO:防重复提交
            //TODO:请求防伪
            //TODO:跨域
            //TODO:格式转换
            //TODO:服务健康检查
            var route = UnicornContext.RouteRule;
            var routeData = UnicornContext.RouteData;
            var dsRoutes = UnicornContext.DownstreamRoutes;
            var tasks = new Task<HttpResponseMessage>[dsRoutes.Length];
            for (int i = 0; i < dsRoutes.Length; i++)
            {
                tasks[i] = HandleDownstreamAsync(context, dsRoutes[i], route, routeData);
            }
            Task.WaitAll(tasks);
            HttpResponseMessage responseMessage;
            if (dsRoutes.Length > 1)
            {
                var responseMessages = new Dictionary<string, HttpResponseMessage>();
                for (int i = 0; i < dsRoutes.Length; i++)
                {
                    responseMessages[dsRoutes[i].DownstreamServiceName] = tasks[i].Result;
                }

                responseMessage = await _aggregateProvider.AggregateAsync(responseMessages, route.AggregateOptions);
            }
            else if (dsRoutes.Length == 1)
            {
                responseMessage = tasks[0].Result;
            }
            else
            {
                await next(context);
                return;
            }
            await WriteResponseAsync(context, responseMessage);
        }

        private async Task<HttpResponseMessage> HandleDownstreamAsync(HttpContext context, DownstreamRoute dsRoute, RouteRule routeRule, RouteData routeData)
        {
            return null;
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpResponseMessage message)
        {
            var headers = message.Content.Headers.Select(r => r);
            foreach (var header in headers)
            {
                context.Response.Headers.Add(header.Key, header.Value.ToArray());
            }
            await message.Content.CopyToAsync(context.Response.Body);
        }
    }
}
