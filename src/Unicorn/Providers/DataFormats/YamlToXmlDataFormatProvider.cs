using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using YamlDotNet.Serialization;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class YamlToXmlDataFormatProvider : IDataFormatProvider
    {
        public const string ProviderName = "YamlToXml";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            var deserializer = new Deserializer();
            var yamlObject = deserializer.Deserialize(new StringReader(data.BodyString));
            var json = JsonConvert.SerializeObject(yamlObject);
            var doc = JsonConvert.DeserializeXmlNode(json);
            data.BodyString = doc.InnerXml;
            data.ContentType = ContentTypes.Application.Xml;
            return Task.FromResult(data);
        }
    }
}
