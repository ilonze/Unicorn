using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Serializers;
using Unicorn.Options;

namespace Unicorn.AggregateProviders
{
    public class JsonMerginAggregateProvider : IAggregateProvider
    {
        private readonly IJsonSerializer _jsonSerializer;
        public JsonMerginAggregateProvider(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }
        public const string ProviderName = "JsonMergin";
        public string Name => ProviderName;

        public async Task<HttpResponseMessage> AggregateAsync(IEnumerable<KeyValuePair<string, HttpResponseMessage>> messages, AggregateOptions aggregateOptions, CancellationToken token = default)
        {
            var data = JObject.FromObject(new { });

            foreach (var message in messages)
            {
                var json = await message.Value.Content.ReadAsStringAsync(token);
                data.Merge(_jsonSerializer.Deserialize<JObject>(json));
            }
            var jsonString = _jsonSerializer.Serialize(data);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };
            await DefaultAggregateProvider.ParseHeaders(messages, result, token);
            return result;
        }
    }
}
