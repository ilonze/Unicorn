using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Unicorn.Options;

namespace Unicorn.Handlers
{
    public interface IRouteHandler
    {
        void Initialization();
        RouteValueDictionary MatchRequestPath(HttpContext context, out RouteRule route);
        string FillRoute(string templatePath, RouteValueDictionary routeData);
        void AddRouteRule(RouteRule route);
        void DeleteRouteRule(string routeKey);
        void SetRouteRule(RouteRule route);
    }
}
