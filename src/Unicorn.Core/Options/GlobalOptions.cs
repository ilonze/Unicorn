using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class GlobalOptions: DownstreamRoute
    {
        public Dictionary<string, StringValues> AddHeadersToRequest { get; set; }
        public Dictionary<string, StringValues> AddHeadersToRequestFromRoute { get; set; }
        public Dictionary<string, StringValues> AddHeadersToResponse { get; set; }
        public Dictionary<string, StringValues> AddHeadersToResponseFromRoute { get; set; }
        public Dictionary<string, StringValues> AddQueriesToRequest { get; set; }
        public Dictionary<string, StringValues> AddQueriesToRequestFromRoute { get; set; }
        public Dictionary<string, StringValues> UpstreamHeaderTransform { get; set; }
        public Dictionary<string, StringValues> DownstreamHeaderTransform { get; set; }
        public Dictionary<int, int> HttpStatusCodeTransform { get; set; }
        public AggregateOptions AggregateOptions { get; set; }
        public QoSOptions QoSOptions { get; set; }
        public LoadBalanceOptions LoadBalancerOptions { get; set; }
        public RateLimitRuleOptions RateLimitRuleOptions { get; set; }
        public AuthenticationOptions AuthenticationOptions { get; set; }
        public HttpHandlerOptions HttpHandlerOptions { get; set; }
        public ABListOptions ABListOptions { get; set; }
        public CacheOptions CacheOptions { get; set; }
        public AntiResubmitOptions AntiResubmitOptions { get; set; }
        public CorsOptions CorsOptions { get; set; }
        public bool RouteIsCaseSensitive { get; set; }
        public int Timeout { get; set; }
        public string HeaderServer { get; set; }
    }
}
