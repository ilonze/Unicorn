using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.RateLimits
{
    public interface IRateLimitProvider: IProvider
    {
        Task<string> CreateKeyAsync(UnicornContext context, CancellationToken token = default);
    }
}
