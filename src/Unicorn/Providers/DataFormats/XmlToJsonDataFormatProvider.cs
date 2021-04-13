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
    public class XmlToJsonDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "XmlToJson";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            var doc = new XmlDocument();
            doc.LoadXml(data.BodyString);
            data.BodyString = JsonConvert.SerializeXmlNode(doc);
            data.ContentType = ContentTypes.Application.Json;
            return Task.FromResult(data);
        }
    }
}
