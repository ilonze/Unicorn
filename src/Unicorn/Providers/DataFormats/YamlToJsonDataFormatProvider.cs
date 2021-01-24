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
    public class YamlToJsonDataFormatProvider: IDataFormatProvider
    {
        public const string ProviderName = "YamlToJson";
        public string Name => ProviderName;

        public Task<ResponseData> ConvertAsync(UnicornContext context, ResponseData data, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<RequestData> ConvertAsync(UnicornContext context, RequestData data, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
