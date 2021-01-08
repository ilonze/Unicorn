using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.RateLimitProviders
{
    public class HeaderRateLimitProvider: IRateLimitProvider
    {
        public const string ProviderName = "Header";
        public string Name => ProviderName;

        public Task<string> CreateKeyAsync(HttpContext context, UnicornContext unicornContext)
        {
            throw new NotImplementedException();
        }
    }
}
