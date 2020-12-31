using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Handlers;
using Unicorn.Options;

namespace Unicorn.Controllers
{
    public class RouteController: BaseController
    {
        private readonly UnicornOptions _options;
        private readonly IRouteHandler _routeHandler;
        private readonly IServiceHandler _serviceHandler;
        public RouteController(
            IOptions<UnicornOptions> options,
            IRouteHandler routeHandler,
            IServiceHandler serviceHandler)
        {
            _options = options.Value;
            _routeHandler = routeHandler;
            _serviceHandler = serviceHandler;
        }

        [HttpPut]
        [Route("route/rule/add")]
        public void AddRouteRule(RouteRule route)
        {
            _routeHandler.AddRouteRule(route);
        }

        [HttpPost]
        [Route("route/rule/set")]
        public void SetRouteRule(RouteRule route)
        {
            _routeHandler.SetRouteRule(route);
        }

        [HttpDelete]
        [Route("route/rule/delete/{routeKey}")]
        public void DeleteRouteRule(string routeKey)
        {
            _routeHandler.DeleteRouteRule(routeKey);
        }
    }
}
