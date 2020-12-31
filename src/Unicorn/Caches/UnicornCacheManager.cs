using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Core;
using Unicorn.Core.Caching;
using Unicorn.Core.Exceptions;
using Unicorn.Options;

namespace Unicorn.Caches
{
    public class UnicornCacheManager : IUnicornCacheManager
    {
        private const long BaseTime = 637161984000000000;
        private readonly IUnicornCacheProvider _distributedCache;
        private readonly IUnicornCacheProvider _memoryCache;

        private readonly UnicornOptions _options;

        public UnicornCacheManager(
            IOptions<UnicornOptions> options,
            DistributedUnicornCacheProvider distributedCache,
            MemoryUnicornCacheProvider memoryCache
            )
        {
            _options = options.Value;
            _distributedCache = distributedCache;
            _memoryCache = memoryCache;
        }

        //public async Task<IEnumerable<RouteRule>> GetRoutesAsync()
        //{
        //    var cache = _options.IsUseDistributedCace ? _distributedCache : _memoryCache;
        //    return await cache.GetAsync<RouteRule[]>(_options.CacheKeyPrefix + "routes");
        //}

        //public async Task SetRoutesAsync(IEnumerable<RouteRule> routes)
        //{
        //    var cache = _options.IsUseDistributedCace ? _distributedCache : _memoryCache;
        //    await cache.SetAsync(_options.CacheKeyPrefix + "routes", routes, DateTimeOffset.MaxValue);
        //}

        //public async Task<IEnumerable<Service>> GetServicesAsync()
        //{
        //    var cache = _options.IsUseDistributedCace ? _distributedCache : _memoryCache;
        //    return await cache.GetAsync<Service[]>(_options.CacheKeyPrefix + "services");
        //}

        //public async Task SetServicesAsync(IEnumerable<Service> services)
        //{
        //    var cache = _options.IsUseDistributedCace ? _distributedCache : _memoryCache;
        //    await cache.SetAsync(_options.CacheKeyPrefix + "services", services, DateTimeOffset.MaxValue);
        //}

        public async Task<Dictionary<string, LoadBalanceData>> GetServicesLoadBalaceDataAsync(string serviceName, IEnumerable<string> serviceIds)
        {
            var cache = _options.IsUseDistributedCace && _options.IsShareLoadBalance ? _distributedCache : _memoryCache;
            var dict = new Dictionary<string, LoadBalanceData>();
            foreach (var serviceId in serviceIds)
            {
                var data = await cache.GetAsync<LoadBalanceData>(_options.CacheKeyPrefix + "loadbalance:" + serviceName + ":" + serviceId);
                dict[serviceId] = data;
            }
            return dict;
        }

        public async Task<bool> SetRateLimitDataAsync(string featureKey, string[] featureValues, TimeSpan expire, int limit)
        {
            var timeKey = _options.CacheKeyPrefix + "ratelimit:times:" + featureKey;
            var valueKey = _options.CacheKeyPrefix + "ratelimit:values:" + featureKey;
            var cache = _options.IsUseDistributedCace && _options.IsShareRateLimit ? _distributedCache : _memoryCache;
            var data = await cache.GetAsync<List<long>>(timeKey) ?? new List<long>();
            var result = data.Count < limit;
            data = data.Where(r => DateTimeOffset.MinValue.AddTicks(r + BaseTime) < DateTimeOffset.Now.Add(-expire)).TakeLast(limit - 1).ToList();
            data.Add(DateTimeOffset.Now.Ticks - BaseTime);
            await cache.SetAsync(timeKey, data, DateTimeOffset.Now.Add(expire));
            await cache.SetAsync(valueKey, featureValues, DateTimeOffset.MaxValue);
            return result;
        }

        public async Task SetServiceLoadBalaceDataAsync(string serviceName, string serviceId, LoadBalanceData data)
        {
            var cache = _options.IsUseDistributedCace && _options.IsShareLoadBalance ? _distributedCache : _memoryCache;
            await cache.SetAsync(_options.CacheKeyPrefix + "loadbalance:" + serviceName + ":" + serviceId, data, DateTimeOffset.MaxValue);
        }
    }
}
