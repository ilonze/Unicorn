using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Core;
using Unicorn.Handlers;
using Unicorn.Options;

namespace Unicorn.Controllers
{
    [Route("api/unicorn")]
    public class UnicornController: BaseController
    {
        private readonly UnicornOptions _options;
        private readonly IRouteHandler _routeHandler;
        private readonly IServiceHandler _serviceHandler;
        public UnicornController(
            IOptions<UnicornOptions> options,
            IRouteHandler routeHandler,
            IServiceHandler serviceHandler)
        {
            _options = options.Value;
            _routeHandler = routeHandler;
            _serviceHandler = serviceHandler;
        }

        [HttpGet]
        [Route("configuration")]
        public UnicornOptions GetOptions()
        {
            return _options;
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

        [HttpPut]
        [Route("service/add")]
        public void AddService(Service service)
        {
            _serviceHandler.AddService(service);
        }

        [HttpPost]
        [Route("service/set")]
        public void SetService(Service service)
        {
            _serviceHandler.SetService(service);
        }

        [HttpPost]
        [Route("service/register")]
        public void RegisterService(Service service)
        {

        }

        [HttpDelete]
        [Route("service/delete/{serviceId}")]
        public void DeleteService(string serviceId)
        {
            _serviceHandler.DeleteService(serviceId);
        }
    }
}
