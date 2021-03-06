﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Providers.Aggregates
{
    [ProviderName(ProviderName)]
    public class TextAggregateProvider : IAggregateProvider
    {
        public const string ProviderName = "Text";
        public string Name => ProviderName;

        public async Task<ResponseData> AggregateAsync(UnicornContext context, AggregateOptions aggregateOptions, CancellationToken token = default)
        {
            var sb = new StringBuilder();
            foreach (var message in context.ResponseMessages)
            {
                var text = await message.Value.Content.ReadAsStringAsync(token);
                sb.Append(text);
            }
            var result = new ResponseData
            {
                BodyString = sb.ToString()
            };
            await DefaultAggregateProvider.ParseHeaders(context.ResponseMessages, result, token);
            return result;
        }
    }
}
