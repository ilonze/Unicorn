using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Options
{
    public class GlobalOptions: DownstreamRoute, IOptions
    {
        public RequestOptions RequestOptions { get; set; }
        public ResponseOptions ResponseOptions { get; set; }
        public AggregateOptions AggregateOptions { get; set; }
        public QoSOptions QoSOptions { get; set; }
        public LoadBalanceOptions LoadBalancerOptions { get; set; }
        public RateLimitOptions RateLimitOptions { get; set; }
        public AuthenticationOptions AuthenticationOptions { get; set; }
        public HttpHandlerOptions HttpHandlerOptions { get; set; }
        public ABListOptions ABListOptions { get; set; }
        public CacheOptions CacheOptions { get; set; }
        public AntiResubmitOptions AntiResubmitOptions { get; set; }
        public CorsOptions CorsOptions { get; set; }
        public EncryptOptions EncryptOptions { get; set; }
        public SignOptions SignOptions { get; set; }
        public bool? RouteIsCaseSensitive { get; set; }
        public int Timeout { get; set; }
    }
}
