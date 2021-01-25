using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
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
        Task<Dictionary<string, LoadBalanceData>> GetServicesLoadBalaceDataAsync(string serviceName, IEnumerable<string> serviceIds, CancellationToken token = default);
        Task SetServiceLoadBalaceDataAsync(string serviceName, string serviceId, LoadBalanceData data, CancellationToken token = default);
        Task<bool> SetRateLimitDataAsync(string featureKey, TimeSpan expiration, int limit, CancellationToken token = default);
        Task SetResponseDataAsync(string featureKey, byte[] body, Dictionary<string, StringValues> headers, TimeSpan seconds, CancellationToken token = default);
        Task SetResponseDataAsync(string featureKey, ResponseData responseData, TimeSpan seconds, CancellationToken token = default);
        Task<ResponseData> GetResponseDataAsync(string featureKey, CancellationToken token = default);
        Task<bool> CheckAntiResubmitAsync(string featureKey, TimeSpan expiration, CancellationToken token = default);
        Task<IEnumerable<string>> FilterQoSAsync(string serviceName, IEnumerable<string> serviceIds, int duration, CancellationToken token = default);
        Task SaveQoSDataAsync(string serviceName, IEnumerable<string> serviceIds, int threshold, int timeout, int duration, CancellationToken token = default);
    }
}
