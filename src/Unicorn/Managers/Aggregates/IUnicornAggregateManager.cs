using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Managers.Aggregates
{
    public interface IUnicornAggregateManager
    {
        Task<ResponseData> AggregateAsync(
            HttpContext httpContext,
            UnicornContext unicornContext,
            CancellationToken token = default);
    }
}
