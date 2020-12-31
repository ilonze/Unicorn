using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.Core.Caching
{
    public class MemoryUnicornCacheProvider : IUnicornCacheProvider
    {
        private ConcurrentDictionary<string, (object Value, DateTimeOffset Expiration)> _cache = new ConcurrentDictionary<string, (object, DateTimeOffset)>();

        public string ProviderName => "Memory";

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            if (_cache.ContainsKey(key))
            {
                if (_cache.TryGetValue(key, out var value) && value.Expiration > DateTimeOffset.Now)
                {
                    return (T)value.Value;
                }
            }
            await Task.CompletedTask;
            return default;
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.TryRemove(key, out var _);
            }
            await Task.CompletedTask;
        }

        public async Task SetAsync<T>(string key, T value, DateTimeOffset expire, CancellationToken token = default)
        {
            _cache.AddOrUpdate(key, (value, expire), (k,v) => (value, expire));
            await Task.CompletedTask;
        }
    }
}
