using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Serializers;
using Unicorn.Options;

namespace Unicorn.AggregateProviders
{
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

        public async Task<HttpResponseMessage> AggregateAsync(IEnumerable<KeyValuePair<string, HttpResponseMessage>> messages, AggregateOptions aggregateOptions, CancellationToken token = default)
        {
            var keys = messages.ToDictionary(r => r.Key, r => aggregateOptions.AggregateKeys.ContainsKey(r.Key) ? aggregateOptions.AggregateKeys[r.Key] : r.Key);
            var datas = messages.ToDictionary(r => keys[r.Key], async r => await ParseData(r.Value, token));
            var json = _jsonSerializer.Serialize(datas);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
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

        internal static async Task ParseHeaders(IEnumerable<KeyValuePair<string, HttpResponseMessage>> messages, HttpResponseMessage responseMessage, CancellationToken token = default)
        {
            var headers = responseMessage.Headers;
            foreach (var message in messages)
            {
                foreach (var keyValue in message.Value.Content.Headers)
                {
                    if (headers.Contains(keyValue.Key))
                    {
                        var oldValues = headers.GetValues(keyValue.Key);
                        headers.Remove(keyValue.Key);
                        headers.Add(keyValue.Key, keyValue.Value.Union(oldValues).Distinct());
                    }
                    else
                    {
                        headers.Add(keyValue.Key, keyValue.Value);
                    }
                }
            }
            await Task.CompletedTask;
        }
    }
}
