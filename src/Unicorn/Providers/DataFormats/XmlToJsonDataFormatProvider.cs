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
    public class XmlToJsonDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "XmlToJson";
        public string Name => ProviderName;

        public Task<ResponseData> ConvertAsync(ResponseData content, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
