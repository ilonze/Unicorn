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
    public class JsonToJsonpDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "JsonToJsonp";
        public string Name => ProviderName;

        public Task<ResponseData> ConvertAsync(ResponseData data, CancellationToken token = default)
        {
            var callback = "callback";
            if (data.HttpContext.Request.Query.ContainsKey(callback))
            {
                callback = data.HttpContext.Request.Query[nameof(callback)];
            }
            data.BodyString = callback + "(" + data.BodyString + ")";
            data.Headers["Content-Type"] = "text/javascript";
            return Task.FromResult(data);
        }

        public Task<RequestData> ConvertAsync(RequestData data, CancellationToken token = default)
        {
            return Task.FromResult(data);
        }
    }
}
