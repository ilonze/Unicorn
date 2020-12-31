//using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Text;
using Unicorn.Options;

namespace Unicorn.Options
{
    public class RouteRule: GlobalOptions
    {
        public string Key { get; set; }
        public string UpstreamRouteTemplate { get; set; }
        //internal TemplateMatcher UpstreamRouteTemplateMatcher { get; set; }
        public List<UpstreamFactor> UpstreamFactors { get; set; }
        public List<DownstreamRoute> DownstreamRoutes { get; set; }
        public int Priority { get; set; }
    }
}
