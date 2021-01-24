using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Providers.Encrypts
{
    [ProviderName(ProviderName)]
    public class DesEncryptProvider : IEncryptProvider
    {
        public const string ProviderName = "Des";
        public string Name => ProviderName;

        public Task<bool> DecryptAsync(UnicornContext context, EncryptOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task EncryptAsync(UnicornContext context, EncryptOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
