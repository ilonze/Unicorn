using Microsoft.AspNetCore.Routing;

namespace Unicorn
{
    public class UnicornContext
    {
        public RouteRule RouteRule { get; set; }
        public RouteValueDictionary RouteData { get; set; }
        public DownstreamRoute[] DownstreamRoutes { get; set; }
        public ResponseData ResponseData { get; set; }
        public RequestData RequestData { get; set; }
    }
}
