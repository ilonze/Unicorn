using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.Caching
{
    public interface IUnicornCacheProvider
    {
        string ProviderName { get; }
        Task<T> GetAsync<T>(string key, CancellationToken token = default);
        Task SetAsync<T>(string key, T value, DateTimeOffset expire, CancellationToken token = default);
        Task RemoveAsync(string key, CancellationToken token = default);
    }
}
