using System.Collections.Generic;
using Unicorn.Options;

namespace Unicorn
{
    public class RouteRule: GlobalOptions
    {
        public string Key { get; set; }
        public string UpstreamRouteTemplate { get; set; }
        public List<UpstreamFactor> UpstreamFactors { get; set; }
        public List<DownstreamRoute> DownstreamRoutes { get; set; }
        public int Priority { get; set; }
    }
}
