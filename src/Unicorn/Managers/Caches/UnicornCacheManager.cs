using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<Dictionary<string, LoadBalanceData>> GetServicesLoadBalaceDataAsync(string serviceName, IEnumerable<string> serviceIds, CancellationToken token = default)
        {
            var cache = Options.UnicornDataUseDistributedCace && Options.IsShareLoadBalance ? DistributedCache : MemoryCache;
            var dict = new Dictionary<string, LoadBalanceData>();
            foreach (var serviceId in serviceIds)
            {
                var data = await cache.GetAsync<LoadBalanceData>(Options.CacheKeyPrefix + "loadbalance:" + serviceName + ":" + serviceId, token);
                dict[serviceId] = data;
            }
            return dict;
        }

        public async Task<bool> SetRateLimitDataAsync(string featureKey, TimeSpan expiration, int limit, CancellationToken token = default)
        {
            var timeKey = Options.CacheKeyPrefix + "ratelimit:" + featureKey;
            var cache = Options.UnicornDataUseDistributedCace && Options.IsShareRateLimit ? DistributedCache : MemoryCache;
            var cacheData = await cache.GetAsync<List<long>>(timeKey, token) ?? new List<long>();
            var data = cacheData.Where(r => DateTimeOffset.MinValue.AddTicks(r + BaseTime) < DateTimeOffset.Now.Add(-expiration)).ToList();
            var isChange = data.Count != cacheData.Count;
            var result = data.Count <= limit;
            if (data.Count >= limit)
            {
                data = data.TakeLast(limit - 1).ToList();
            }
            data.Add(DateTimeOffset.Now.Ticks - BaseTime);
            await cache.SetAsync(timeKey, data, DateTimeOffset.Now.Add(expiration), token);
            return result;
        }

        public async Task SetServiceLoadBalaceDataAsync(string serviceName, string serviceId, LoadBalanceData data, CancellationToken token = default)
        {
            var cache = Options.UnicornDataUseDistributedCace && Options.IsShareLoadBalance ? DistributedCache : MemoryCache;
            await cache.SetAsync(Options.CacheKeyPrefix + "loadbalance:" + serviceName + ":" + serviceId, data, DateTimeOffset.MaxValue, token);
        }

        public async Task SetResponseDataAsync(string featureKey, byte[] body, Dictionary<string, StringValues> headers, TimeSpan expiration, CancellationToken token = default)
        {
            await SetResponseDataAsync(featureKey, new ResponseData { Body = body, Headers = headers }, expiration, token);
        }

        public async Task SetResponseDataAsync(string featureKey, ResponseData responseData, TimeSpan expiration, CancellationToken token = default)
        {
            var cache = Options.ResponseDataUseDistributedCace ? DistributedCache : MemoryCache;
            await cache.SetAsync(Options.CacheKeyPrefix + "response:" + featureKey, responseData, DateTimeOffset.Now.Add(expiration), token);
        }

        public async Task<ResponseData> GetResponseDataAsync(string featureKey, CancellationToken token = default)
        {
            var cache = Options.ResponseDataUseDistributedCace ? DistributedCache : MemoryCache;
            return await cache.GetAsync<ResponseData>(Options.CacheKeyPrefix + "response:" + featureKey, token);
        }

        public async Task<bool> CheckAntiResubmitAsync(string featureKey, TimeSpan expiration, CancellationToken token = default)
        {
            var cache = Options.AntiResubmitDataUseDistributedCace ? DistributedCache : MemoryCache;
            var key = Options.CacheKeyPrefix + "antiresubmit:" + featureKey;
            var result = (await cache.GetAsync<string>(key, token)) != "1";
            await cache.SetAsync(key, "1", DateTimeOffset.Now.Add(expiration));
            return result;
        }

        public async Task<IEnumerable<string>> FilterQoSAsync(string serviceName, IEnumerable<string> serviceIds, int duration, CancellationToken token = default)
        {
            var cache = Options.QoSDataUseDistributedCace ? DistributedCache : MemoryCache;
            var key = Options.CacheKeyPrefix + "qos-break:" + serviceName;
            var data = (await cache.GetAsync<Dictionary<string, long>>(key, token)) ?? new Dictionary<string, long>();
            var list = new List<string>();
            var needSave = false;
            foreach (var serviceId in serviceIds)
            {
                if(!data.ContainsKey(serviceId) || DateTimeOffset.MinValue.AddTicks(data[serviceId] + BaseTime) < DateTimeOffset.Now.AddSeconds(-duration))
                {
                    list.Add(serviceId);
                    if (data.ContainsKey(serviceId))
                    {
                        data.Remove(serviceId);
                        needSave = true;
                    }
                }
            }
            if (needSave)
            {
                await cache.SetAsync(key, data, DateTimeOffset.Now.AddSeconds(duration), token);
            }
            return list;
        }

        public async Task SaveQoSDataAsync(string serviceName, IEnumerable<string> serviceIds, int threshold, int timeout, int duration, CancellationToken token = default)
        {
            var cache = Options.QoSDataUseDistributedCace ? DistributedCache : MemoryCache;
            var key = Options.CacheKeyPrefix + "qos:" + serviceName;
            var result = (await cache.GetAsync<Dictionary<string, List<long>>>(key, token)) ?? new Dictionary<string, List<long>>();
            var now = DateTimeOffset.Now.Ticks - BaseTime;
            foreach (var serviceId in serviceIds)
            {
                if (!result.ContainsKey(serviceId))
                {
                    result[serviceId] = new List<long> { now };
                }
                else
                {
                    result[serviceId].Add(now);
                }
            }
            var breakKey = Options.CacheKeyPrefix + "qos-break:" + serviceName;
            Dictionary<string, long> data = null;
            async Task<Dictionary<string, long>> GetDataAsunc()
            {
                return (await cache.GetAsync<Dictionary<string, long>>(breakKey, token)) ?? new Dictionary<string, long>();
            }
            foreach (var serviceId in result.Keys.ToArray())
            {
                var list = result[serviceId];
                list = list.Where(r => DateTimeOffset.MinValue.AddTicks(r + BaseTime) > DateTimeOffset.Now.AddSeconds(-timeout)).ToList();
                if (list.Count > 0)
                {
                    result[serviceId] = list;
                    if (result[serviceId].Count >= threshold)
                    {
                        result.Remove(serviceId);
                        data ??= await GetDataAsunc();
                        data[serviceId] = now;
                    }
                }
                else
                {
                    result.Remove(serviceId);
                }
            }
            if (data != null)
            {
                await cache.SetAsync(breakKey, data, DateTimeOffset.Now.AddSeconds(duration), token);
            }
            await cache.SetAsync(key, result, DateTimeOffset.Now.AddSeconds(timeout), token);
        }
    }
}
