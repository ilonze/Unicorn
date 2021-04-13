using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Unicorn.Datas;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class JsonToXmlDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "JsonToXml";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            var doc = JsonConvert.DeserializeXmlNode(data.BodyString);
            data.BodyString = doc.InnerXml;
            data.ContentType = ContentTypes.Application.Xml;
            return Task.FromResult(data);
        }
    }
}
