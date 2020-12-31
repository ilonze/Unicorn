using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Core.Serializers;

namespace Unicorn.Core.Caching
{
    public class DistributedUnicornCacheProvider: IUnicornCacheProvider
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IDistributedCacheSerializer _distributedCacheSerializer;
        public DistributedUnicornCacheProvider(
            IDistributedCache distributedCache,
            IDistributedCacheSerializer distributedCacheSerializer
            )
        {
            _distributedCache = distributedCache;
            _distributedCacheSerializer = distributedCacheSerializer;
        }

        public string ProviderName => "Distributed";

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            var bytes = await _distributedCache.GetAsync(key, token);
            return _distributedCacheSerializer.Deserialize<T>(bytes);
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, DateTimeOffset expire, CancellationToken token = default)
        {
            var bytes = _distributedCacheSerializer.Serialize(value);
            await _distributedCache.SetAsync(key, bytes, token);
        }
    }
}
