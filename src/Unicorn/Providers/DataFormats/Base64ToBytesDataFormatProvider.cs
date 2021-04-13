using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class Base64ToBytesDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "Base64ToBytes";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            data.Body = Convert.FromBase64String(data.BodyString);
            data.BodyString = null;
            data.ContentType = ContentTypes.Application.OctetStream;
            return Task.FromResult(data);
        }
    }
}
