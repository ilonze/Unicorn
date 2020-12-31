using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Core;
using Unicorn.Options;

namespace Unicorn.Caches
{
    public interface IUnicornCacheManager
    {
        //Task<IEnumerable<RouteRule>> GetRoutesAsync();
        //Task SetRoutesAsync(IEnumerable<RouteRule> routes);
        //Task<IEnumerable<Service>> GetServicesAsync();
        //Task SetServicesAsync(IEnumerable<Service> services);
        Task<Dictionary<string, LoadBalanceData>> GetServicesLoadBalaceDataAsync(string serviceName, IEnumerable<string> serviceIds);
        Task SetServiceLoadBalaceDataAsync(string serviceName, string serviceId, LoadBalanceData data);
        Task<bool> SetRateLimitDataAsync(string featureKey, string[] featureValues, TimeSpan expire, int limit);
    }
}
