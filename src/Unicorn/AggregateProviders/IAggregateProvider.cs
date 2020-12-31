﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Core;
using Unicorn.Options;

namespace Unicorn.AggregateProviders
{
    public interface IAggregateProvider: IProvider
    {
        Task<HttpResponseMessage> AggregateAsync(IEnumerable<KeyValuePair<string, HttpResponseMessage>> messages, AggregateOptions aggregateOptions, CancellationToken token = default);
    }
}
