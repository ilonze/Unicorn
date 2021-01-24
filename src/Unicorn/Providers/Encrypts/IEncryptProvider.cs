using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Providers.Encrypts
{
    public interface IEncryptProvider: IProvider
    {
        Task EncryptAsync(UnicornContext context, EncryptOptions options, CancellationToken token = default);
        Task<bool> DecryptAsync(UnicornContext context, EncryptOptions options, CancellationToken token = default);
    }
}
