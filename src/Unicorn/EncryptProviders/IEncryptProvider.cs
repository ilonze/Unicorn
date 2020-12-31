using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Core;

namespace Unicorn.EncryptProviders
{
    public interface IEncryptProvider: IProvider
    {
        Task<byte[]> EncryptAsync(byte[] data, CancellationToken token = default);
        Task<byte[]> DecryptAsync(byte[] data, CancellationToken token = default);
    }
}
