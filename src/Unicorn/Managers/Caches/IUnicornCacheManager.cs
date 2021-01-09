using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Managers.Caches
{
    public interface IUnicornCacheManager
    {
        //Task<IEnumerable<RouteRule>> GetRoutesAsync();
        //Task SetRoutesAsync(IEnumerable<RouteRule> routes);
        //Task<IEnumerable<Service>> GetServicesAsync();
        //Task SetServicesAsync(IEnumerable<Service> services);
        Task<Dictionary<string, LoadBalanceData>> GetServicesLoadBalaceDataAsync(string serviceName, IEnumerable<string> serviceIds);
        Task SetServiceLoadBalaceDataAsync(string serviceName, string serviceId, LoadBalanceData data);
        Task<bool> SetRateLimitDataAsync(string featureKey, TimeSpan expiration, int limit);
        Task SetResponseDataAsync(string featureKey, byte[] body, Dictionary<string, StringValues> headers, TimeSpan seconds);
        Task SetResponseDataAsync(string featureKey, ResponseData responseData, TimeSpan seconds);
        Task<ResponseData> GetResponseDataAsync(string featureKey);
        Task<bool> CheckAntiResubmitAsync(string featureKey, TimeSpan expiration);
    }
}
