using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.SignProviders
{
    public class RsaSignProvider : ISignProvider
    {
        public const string ProviderName = "Rsa";
        public string Name => ProviderName;

        public Task<HttpContent> Sign(HttpContent content, SignOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Verify(HttpContext content, SignOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
