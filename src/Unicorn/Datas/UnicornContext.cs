using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Net.Http;

namespace Unicorn.Datas
{
    public class UnicornContext
    {
        public RouteRule RouteRule { get; set; }
        public RouteValueDictionary RouteData { get; set; }
        public RequestData RequestData { get; set; }
        public DownstreamRoute[] DownstreamRoutes { get; set; }
        public Dictionary<string, HttpResponseMessage> ResponseMessages { get; set; }
        public ResponseData ResponseData { get; set; }
        public HttpContext HttpContext { get; set; }
    }
}
