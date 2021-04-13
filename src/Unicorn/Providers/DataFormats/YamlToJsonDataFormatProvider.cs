using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using YamlDotNet.Serialization;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class YamlToJsonDataFormatProvider: IDataFormatProvider
    {
        public const string ProviderName = "YamlToJson";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            var deserializer = new Deserializer();
            var yamlObject = deserializer.Deserialize(new StringReader(data.BodyString));
            data.BodyString = JsonConvert.SerializeObject(yamlObject);
            data.ContentType = ContentTypes.Application.Json;
            return Task.FromResult(data);
        }
    }
}
