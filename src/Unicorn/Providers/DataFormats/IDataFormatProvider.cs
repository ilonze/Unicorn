﻿using System;
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
        Task<RequestData> ConvertAsync(UnicornContext context, RequestData data, CancellationToken token = default);
        Task<ResponseData> ConvertAsync(UnicornContext context, ResponseData data, CancellationToken token = default);
    }
}
