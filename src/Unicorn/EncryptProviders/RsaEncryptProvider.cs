using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.EncryptProviders
{
    public class RsaEncryptProvider : IEncryptProvider
    {
        public const string ProviderName = "Rsa";
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
