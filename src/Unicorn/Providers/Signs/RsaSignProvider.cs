using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Providers.Signs
{
    [ProviderName(ProviderName)]
    public class RsaSignProvider : ISignProvider
    {
        public const string ProviderName = "Rsa";
        public string Name => ProviderName;

        public Task SignAsync(UnicornContext context, SignOptions options, CancellationToken token = default)
        {
            //TODO:NotImplemented
            throw new NotImplementedException();
        }

        public Task<bool> VerifyAsync(UnicornContext context, SignOptions options, CancellationToken token = default)
        {
            //TODO:NotImplemented
            throw new NotImplementedException();
        }
    }
}
