using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Caches;

namespace Unicorn.Options
{
    public class UnicornContext
    {
        public RouteRule RouteRule { get; set; }
        public RouteValueDictionary RouteData { get; set; }
        public DownstreamRoute[] DownstreamRoutes { get; set; }
        public ResponseData ResponseData { get; set; }
    }
}
