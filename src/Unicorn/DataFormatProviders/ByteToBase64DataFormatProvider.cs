using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.DataFormatConverters
{
    public class ByteToBase64DataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "ByteToBase64";
        public string Name => ProviderName;

        public Task<HttpContent> ConvertAsync(HttpContent content, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
