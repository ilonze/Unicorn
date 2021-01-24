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
    public interface ISignProvider : IProvider
    {
        Task SignAsync(UnicornContext context, SignOptions options, CancellationToken token = default);
        Task<bool> VerifyAsync(UnicornContext context, SignOptions options, CancellationToken token = default);
    }
}
