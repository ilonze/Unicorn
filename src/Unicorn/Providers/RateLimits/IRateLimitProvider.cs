using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.RateLimits
{
    public interface IRateLimitProvider: IProvider
    {
        Task<string> CreateKeyAsync(HttpContext context, UnicornContext unicornContext);
    }
}
