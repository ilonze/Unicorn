using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Serializers;
using Unicorn.Options;
using Unicorn.Datas;

namespace Unicorn.Providers.Aggregates
{
    [ProviderName(ProviderName)]
    public class DefaultAggregateProvider : IAggregateProvider
    {
        private readonly IJsonSerializer _jsonSerializer;
        public DefaultAggregateProvider(
            IJsonSerializer jsonSerializer
            )
        {
            _jsonSerializer = jsonSerializer;
        }
        public const string ProviderName = "Default";
        public string Name => ProviderName;

        public async Task<ResponseData> AggregateAsync(IEnumerable<KeyValuePair<string, HttpResponseMessage>> messages, AggregateOptions aggregateOptions, CancellationToken token = default)
        {
            var keys = messages.ToDictionary(r => r.Key, r => aggregateOptions.AggregateKeys.ContainsKey(r.Key) ? aggregateOptions.AggregateKeys[r.Key] : r.Key);
            var datas = messages.ToDictionary(r => keys[r.Key], async r => await ParseData(r.Value, token));
            var json = _jsonSerializer.Serialize(datas);
            var result = new ResponseData
            {
                BodyString = json
            };
            await ParseHeaders(messages, result, token);
            return result;
        }

        private async Task<object> ParseData(HttpResponseMessage message, CancellationToken token = default)
        {
            if (message.Content.Headers.ContentType?.MediaType?.Contains("json") == true)
            {
                var data = await message.Content.ReadAsStringAsync(token);
                return _jsonSerializer.Deserialize<object>(data);
            }
            else if (message.Content.Headers.ContentType?.MediaType?.Contains("text") == true)
            {
                var data = await message.Content.ReadAsStringAsync(token);
                return data;
            }
            else
            {
                var bytes = await message.Content.ReadAsByteArrayAsync(token);
                return "data:" + message.Content.Headers.ContentType.MediaType + ";base64," + Encoding.UTF8.GetString(bytes);
            }
        }

        internal static async Task ParseHeaders(IEnumerable<KeyValuePair<string, HttpResponseMessage>> messages, ResponseData responseData, CancellationToken token = default)
        {
            var headers = responseData.Headers;
            foreach (var message in messages)
            {
                foreach (var keyValue in message.Value.Content.Headers)
                {
                    if (headers.ContainsKey(keyValue.Key))
                    {
                        var oldValues = headers[keyValue.Key];
                        headers[keyValue.Key] = keyValue.Value.Union(oldValues).Distinct().ToArray();
                    }
                    else
                    {
                        headers[keyValue.Key] = keyValue.Value.ToArray();
                    }
                }
            }
            await Task.CompletedTask;
        }
    }
}
