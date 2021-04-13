using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Unicorn.Datas;
using YamlDotNet.Serialization;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class XmlToYamlDataFormatProvider: IDataFormatProvider
    {
        public const string ProviderName = "XmlToYaml";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            var doc = new XmlDocument();
            doc.LoadXml(data.BodyString);
            var json = JsonConvert.SerializeXmlNode(doc);
            var jsonObject = JsonConvert.DeserializeObject(json);
            var serializer = new Serializer();
            data.BodyString = serializer.Serialize(jsonObject);
            data.ContentType = ContentTypes.Application.Yaml;
            return Task.FromResult(data);
        }
    }
}
