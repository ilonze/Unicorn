using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Providers.Aggregates
{
    public interface IAggregateProvider: IProvider
    {
        Task<ResponseData> AggregateAsync(UnicornContext context, AggregateOptions aggregateOptions, CancellationToken token = default);
    }
}
