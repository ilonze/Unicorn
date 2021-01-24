using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class Base64ToBytesDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "Base64ToBytes";
        public string Name => ProviderName;

        public Task<RequestData> ConvertAsync(UnicornContext context, RequestData data, CancellationToken token = default)
        {
            data.Body = Convert.FromBase64String(data.Text);
            data.Text = null;
            data.Headers["Content-Type"] = "application/octet-stream";
            return Task.FromResult(data);
        }

        public Task<ResponseData> ConvertAsync(UnicornContext context, ResponseData data, CancellationToken token = default)
        {
            data.Body = Convert.FromBase64String(data.BodyString);
            data.BodyString = null;
            data.ContentType = "application/octet-stream";
            return Task.FromResult(data);
        }
    }
}
