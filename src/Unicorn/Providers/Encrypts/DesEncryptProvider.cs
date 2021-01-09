using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.Providers.Encrypts
{
    [ProviderName(ProviderName)]
    public class DesEncryptProvider : IEncryptProvider
    {
        public const string ProviderName = "Des";
        public string Name => ProviderName;

        public Task<byte[]> DecryptAsync(byte[] data, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> EncryptAsync(byte[] data, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
