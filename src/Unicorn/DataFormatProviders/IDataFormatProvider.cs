using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Core;

namespace Unicorn.DataFormatConverters
{
    public interface IDataFormatProvider: IProvider
    {
        Task<HttpContent> ConvertAsync(HttpContent content, CancellationToken token = default);
    }
}
