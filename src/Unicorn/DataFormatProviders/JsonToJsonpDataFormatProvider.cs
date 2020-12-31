using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.DataFormatConverters
{
    public class JsonToJsonpDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "JsonToJsonp";
        public string Name => ProviderName;

        public Task<HttpContent> ConvertAsync(HttpContent content, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
