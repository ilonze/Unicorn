using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class JsonToYamlDataFormatProvider: IDataFormatProvider
    {
        public const string ProviderName = "JsonToYaml";
        public string Name => ProviderName;

        public Task<HttpContent> ConvertAsync(HttpContent content, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
