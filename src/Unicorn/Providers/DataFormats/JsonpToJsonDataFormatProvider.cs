using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class JsonpToJsonDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "JsonpToJson";
        public string Name => ProviderName;

        public Task<ResponseData> ConvertAsync(UnicornContext context, ResponseData data, CancellationToken token = default)
        {
            data.BodyString = data.BodyString[(data.BodyString.IndexOf("(") + 1)..].TrimEnd(';').TrimEnd(')');
            data.ContentType = "application/json";
            return Task.FromResult(data);
        }

        public Task<RequestData> ConvertAsync(UnicornContext context, RequestData data, CancellationToken token = default)
        {
            return Task.FromResult(data);
        }
    }
}
