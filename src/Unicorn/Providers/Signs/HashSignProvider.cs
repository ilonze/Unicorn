using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Providers.Signs
{
    [ProviderName(ProviderName)]
    public class HashSignProvider: ISignProvider
    {
        public const string ProviderName = "Hash";
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
                data.Headers[options.ResponseHeader] = data.BodyString.Hash();
            }
            else if (data.Body != null && data.Body.Length > 0)
            {
                data.Headers[options.ResponseHeader] = data.Body.Hash();
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
            if (!data.Json.IsNullOrWhiteSpace())
            {
                return Task.FromResult(data.Json.Hash() == data.Headers[options.RequestHeader]);
            }
            if (!data.Text.IsNullOrWhiteSpace())
            {
                return Task.FromResult(data.Text.Hash() == data.Headers[options.RequestHeader]);
            }
            if (data.Body != null && data.Body.Length > 0)
            {
                return Task.FromResult(data.Body.Hash() == data.Headers[options.RequestHeader]);
            }
            return Task.FromResult(false);
        }
    }
}
