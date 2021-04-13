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
    public class Md5SignProvider : ISignProvider
    {
        public const string ProviderName = "Md5";
        public string Name => ProviderName;

        public Task SignAsync(UnicornContext context, SignOptions options, CancellationToken token = default)
        {
            if (token.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }
            var data = context.ResponseData;
            if (!data.BodyString.IsNullOrWhiteSpace())
            {
                data.Headers[options.ResponseHeader] = data.BodyString.Md5();
            }
            else if (data.Body != null && data.Body.Length > 0)
            {
                data.Headers[options.ResponseHeader] = data.Body.Md5();
            }
            return Task.CompletedTask;
        }

        public Task<bool> VerifyAsync(UnicornContext context, SignOptions options, CancellationToken token = default)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromResult(false);
            }
            var data = context.RequestData;
            if (!data.BodyString.IsNullOrWhiteSpace())
            {
                return Task.FromResult(data.BodyString.Md5() == data.Headers[options.RequestHeader]);
            }
            if (data.Body != null && data.Body.Length > 0)
            {
                return Task.FromResult(data.Body.Md5() == data.Headers[options.RequestHeader]);
            }
            return Task.FromResult(false);
        }
    }
}
