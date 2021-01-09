using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class BytesToBase64DataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "BytesToBase64";
        public string Name => ProviderName;

        public Task<ResponseData> ConvertAsync(ResponseData data, CancellationToken token = default)
        {
            data.BodyString = Convert.ToBase64String(data.Body);
            data.Body = null;
            data.Headers["Content-Type"] = "text/plain";
            return Task.FromResult(data);
        }

        public Task<RequestData> ConvertAsync(RequestData data, CancellationToken token = default)
        {
            data.Text = Convert.ToBase64String(data.Body);
            data.Body = null;
            data.Headers["Content-Type"] = "text/plain";
            return Task.FromResult(data);
        }
    }
}
