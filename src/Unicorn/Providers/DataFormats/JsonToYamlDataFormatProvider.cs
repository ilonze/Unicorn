using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using YamlDotNet.Serialization;

namespace Unicorn.Providers.DataFormats
{
    [ProviderName(ProviderName)]
    public class JsonToYamlDataFormatProvider: IDataFormatProvider
    {
        public const string ProviderName = "JsonToYaml";
        public string Name => ProviderName;

        public Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default) where TData : RequestResponseDataBase
        {
            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(data.BodyString);
            var serializer = new Serializer();
            data.BodyString = serializer.Serialize(jsonObject);
            data.ContentType = ContentTypes.Application.Yaml;
            return Task.FromResult(data);
        }
    }
}
