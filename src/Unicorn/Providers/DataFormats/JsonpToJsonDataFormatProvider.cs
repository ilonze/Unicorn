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

        public Task<ResponseData> ConvertAsync(ResponseData content, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
