using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class GlobalOptions: DownstreamRoute
    {
        public AggregateOptions AggregateOptions { get; set; }
        public Dictionary<string, string> AddHeadersToRequest { get; set; }
        public Dictionary<string, string> AddHeadersToRequestFromRoute { get; set; }
        public Dictionary<string, string> AddHeadersToResponse { get; set; }
        public Dictionary<string, string> AddHeadersToResponseFromRoute { get; set; }
        public Dictionary<string, string> AddQueriesToRequest { get; set; }
        public Dictionary<string, string> AddQueriesToRequestFromRoute { get; set; }
        public Dictionary<string, string> UpstreamHeaderTransform { get; set; }
        public Dictionary<string, string> DownstreamHeaderTransform { get; set; }
        public Dictionary<int, int> HttpStatusCodeTransform { get; set; }
        public QoSOptions QoS { get; set; }
        public LoadBalanceOptions LoadBalancerOptions { get; set; }
        public RateLimitRuleOptions RateLimitRuleOptions { get; set; }
        public AuthenticationOptions AuthenticationOptions { get; set; }
        public HttpHandlerOptions HttpHandlerOptions { get; set; }
        public SecurityOptions SecurityOptions { get; set; }
        public CacheOptions CacheOptions { get; set; }
        public bool RouteIsCaseSensitive { get; set; }
        public int Timeout { get; set; }
        public string HeaderServer { get; set; }
    }
}
