using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.AggregateProviders
{
    public class TextAggregateProvider : IAggregateProvider
    {
        public const string ProviderName = "Text";
        public string Name => ProviderName;

        public async Task<HttpResponseMessage> AggregateAsync(IEnumerable<KeyValuePair<string, HttpResponseMessage>> messages, AggregateOptions aggregateOptions, CancellationToken token = default)
        {
            var sb = new StringBuilder();
            foreach (var message in messages)
            {
                var text = await message.Value.Content.ReadAsStringAsync(token);
                sb.Append(text);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(sb.ToString(), Encoding.UTF8, "text/plain")
            };
            await DefaultAggregateProvider.ParseHeaders(messages, result, token);
            return result;
        }
    }
}
