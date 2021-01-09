using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.Providers.Encrypts
{
    [ProviderName(ProviderName)]
    public class AesEncryptProvider : IEncryptProvider
    {
        public const string ProviderName = "Aes";
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
