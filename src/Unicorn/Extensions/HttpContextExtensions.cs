using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Handlers;
using Unicorn.Options;

namespace Unicorn.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetRequestOrigin(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host.Value}";
        }
        public static string GetRequestUrl(this HttpContext context)
        {
            return $"{context.GetRequestOrigin()}{context.Request.Path.Value}{(context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "")}";
        }
        public static string GetClientIp(this HttpContext context)
        {
            var headers = context.Request.Headers;
            if (headers.ContainsKey("X-Forwarded-For") && headers["X-Forwarded-For"].Count > 0)
            {
                return headers["X-Forwarded-For"].ToString();
            }
            else if (headers.ContainsKey("X-Real-IP") && headers["X-Real-IP"].Count > 0)
            {
                return headers["X-Real-IP"].ToString();
            }
            else
            {
                return context.Connection.RemoteIpAddress.ToString();
            }
        }

        public static bool MatchRoute(this HttpContext context)
        {
            var routeHandler = context.RequestServices.GetRequiredService<IRouteHandler>();
            var routeData = routeHandler.MatchRequestPath(context, out var route);
            if (route != null)
            {
                var unicornContext = context.RequestServices.GetRequiredService<UnicornContext>();
                unicornContext.RouteRule = route;
                unicornContext.RouteData = routeData;
                unicornContext.DownstreamRoutes = route.DownstreamRoutes.SelectMany(r => r.DownstreamFactors.Select(f => new DownstreamRoute
                {
                    DownstreamHttpMethod = f.HttpMethod,
                    DownstreamRouteTemplate = r.DownstreamRouteTemplate,
                    DownstreamSchema = f.Schema,
                    DownstreamServiceName = f.ServiceName,
                    DownstreamServiceNamespace = f.ServiceNamespace
                })).ToArray();
            }
            return route != null;
        }
    }
}
