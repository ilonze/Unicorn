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
    public interface ISignProvider : IProvider
    {
        Task<HttpContent> Sign(HttpContent content, SignOptions options, CancellationToken token = default);
        Task<bool> Verify(HttpContext content, SignOptions options, CancellationToken token = default);
    }
}
