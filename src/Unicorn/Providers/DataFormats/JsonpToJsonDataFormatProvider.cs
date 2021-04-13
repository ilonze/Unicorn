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

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            if (data is ResponseData)
            {
                data.BodyString = data.BodyString[(data.BodyString.IndexOf("(") + 1)..].TrimEnd(';').TrimEnd(')');
                data.ContentType = ContentTypes.Application.Json;
            }
            return Task.FromResult(data);
        }
    }
}
