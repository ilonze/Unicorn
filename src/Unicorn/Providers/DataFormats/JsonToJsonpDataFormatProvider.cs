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

        public Task<ResponseData> ConvertAsync(UnicornContext context, ResponseData data, CancellationToken token = default)
        {
            var callback = "callback";
            if (context.HttpContext.Request.Query.ContainsKey(callback))
            {
                callback = context.HttpContext.Request.Query[nameof(callback)];
            }
            data.BodyString = callback + "(" + data.BodyString + ")";
            data.ContentType = "text/javascript";
            return Task.FromResult(data);
        }

        public Task<RequestData> ConvertAsync(UnicornContext context, RequestData data, CancellationToken token = default)
        {
            return Task.FromResult(data);
        }
    }
}
