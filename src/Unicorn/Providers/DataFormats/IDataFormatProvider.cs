using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.DataFormats
{
    public interface IDataFormatProvider: IProvider
    {
        Task<TData> ConvertAsync<TData>(UnicornContext context, TData data, CancellationToken token = default)
            where TData: RequestResponseDataBase;
    }
}
