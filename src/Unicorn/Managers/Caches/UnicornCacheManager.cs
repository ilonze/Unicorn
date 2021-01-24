using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Caching;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Managers.Caches
{
    public class UnicornCacheManager : IUnicornCacheManager
    {
        private const long BaseTime = 637161984000000000;
        protected IUnicornCacheProvider DistributedCache { get; }
        protected IUnicornCacheProvider MemoryCache { get; }
        protected UnicornOptions Options { get; }

        public UnicornCacheManager(
            IOptions<UnicornOptions> options,
            DistributedUnicornCacheProvider distributedCache,
            MemoryUnicornCacheProvider memoryCache
            )
        {
            Options = options.Value;
            DistributedCache = distributedCache;
            MemoryCache = memoryCache;
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
            var cache = Options.UnicornDataUseDistributedCace && Options.IsShareLoadBalance ? DistributedCache : MemoryCache;
            var dict = new Dictionary<string, LoadBalanceData>();
            foreach (var serviceId in serviceIds)
            {
                var data = await cache.GetAsync<LoadBalanceData>(Options.CacheKeyPrefix + "loadbalance:" + serviceName + ":" + serviceId);
                dict[serviceId] = data;
            }
            return dict;
        }

        public async Task<bool> SetRateLimitDataAsync(string featureKey, TimeSpan expiration, int limit)
        {
            var timeKey = Options.CacheKeyPrefix + "ratelimit:" + featureKey;
            var cache = Options.UnicornDataUseDistributedCace && Options.IsShareRateLimit ? DistributedCache : MemoryCache;
            var cacheData = await cache.GetAsync<List<long>>(timeKey) ?? new List<long>();
            var data = cacheData.Where(r => DateTimeOffset.MinValue.AddTicks(r + BaseTime) < DateTimeOffset.Now.Add(-expiration)).ToList();
            var isChange = data.Count != cacheData.Count;
            var result = data.Count <= limit;
            if (data.Count >= limit)
            {
                data = data.TakeLast(limit - 1).ToList();
            }
            data.Add(DateTimeOffset.Now.Ticks - BaseTime);
            await cache.SetAsync(timeKey, data, DateTimeOffset.Now.Add(expiration));
            return result;
        }

        public async Task SetServiceLoadBalaceDataAsync(string serviceName, string serviceId, LoadBalanceData data)
        {
            var cache = Options.UnicornDataUseDistributedCace && Options.IsShareLoadBalance ? DistributedCache : MemoryCache;
            await cache.SetAsync(Options.CacheKeyPrefix + "loadbalance:" + serviceName + ":" + serviceId, data, DateTimeOffset.MaxValue);
        }

        public async Task SetResponseDataAsync(string featureKey, byte[] body, Dictionary<string, StringValues> headers, TimeSpan expiration)
        {
            await SetResponseDataAsync(featureKey, new ResponseData { Body = body, Headers = headers }, expiration);
        }

        public async Task SetResponseDataAsync(string featureKey, ResponseData responseData, TimeSpan expiration)
        {
            var cache = Options.ResponseDataUseDistributedCace ? DistributedCache : MemoryCache;
            await cache.SetAsync(Options.CacheKeyPrefix + "response:" + featureKey, responseData, DateTimeOffset.Now.Add(expiration));
        }

        public async Task<ResponseData> GetResponseDataAsync(string featureKey)
        {
            var cache = Options.ResponseDataUseDistributedCace ? DistributedCache : MemoryCache;
            return await cache.GetAsync<ResponseData>(Options.CacheKeyPrefix + "response:" + featureKey);
        }

        public async Task<bool> CheckAntiResubmitAsync(string featureKey, TimeSpan expiration)
        {
            var cache = Options.AntiResubmitDataUseDistributedCace ? DistributedCache : MemoryCache;
            var key = Options.CacheKeyPrefix + "antiresubmit:" + featureKey;
            var result = (await cache.GetAsync<string>(key)) != "1";
            await cache.SetAsync(key, "1", DateTimeOffset.Now.Add(expiration));
            return result;
        }
    }
}
