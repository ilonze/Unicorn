using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.Providers.Aggregates
{
    [ProviderName(ProviderName)]
    public class ByteAggregateProvider : IAggregateProvider
    {
        public const string ProviderName = "Byte";
        public string Name => ProviderName;

        public async Task<HttpResponseMessage> AggregateAsync(IEnumerable<KeyValuePair<string, HttpResponseMessage>> messages, AggregateOptions aggregateOptions, CancellationToken token = default)
        {
            List<byte> bytes = new List<byte>();

            foreach (var message in messages)
            {
                var ds = await message.Value.Content.ReadAsByteArrayAsync(token);
                bytes.AddRange(ds);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes.ToArray())
            };
            await DefaultAggregateProvider.ParseHeaders(messages, result, token);
            return result;
        }
    }
}
