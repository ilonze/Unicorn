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
    public class JsonToXmlDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "JsonToXml";
        public string Name => ProviderName;

        public Task<ResponseData> ConvertAsync(ResponseData data, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<RequestData> ConvertAsync(RequestData data, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
