using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task SetResponseDataAsync(string featureKey, byte[] data, Dictionary<string, string> headers, int seconds);
        Task SetResponseDataAsync(string featureKey, ResponseData responseData, int seconds);
        Task<ResponseData> GetResponseDataAsync(string featureKey);
    }
}
