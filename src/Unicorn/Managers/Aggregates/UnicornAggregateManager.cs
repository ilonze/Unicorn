using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Managers.Aggregates
{
    public class UnicornAggregateManager : IUnicornAggregateManager
    {
        public Task<ResponseData> AggregateAsync(HttpContext httpContext, UnicornContext unicornContext, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
