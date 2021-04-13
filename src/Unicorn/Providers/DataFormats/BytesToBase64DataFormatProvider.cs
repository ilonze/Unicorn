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
    public class BytesToBase64DataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "BytesToBase64";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            data.BodyString = Convert.ToBase64String(data.Body);
            data.Body = null;
            data.ContentType = ContentTypes.Text.Plain;
            return Task.FromResult(data);
        }
    }
}
