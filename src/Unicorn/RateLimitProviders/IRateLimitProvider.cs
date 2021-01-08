using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.RateLimitProviders
{
    public interface IRateLimitProvider: IProvider
    {
        Task<string> CreateKeyAsync(HttpContext context, UnicornContext unicornContext);
    }
}
