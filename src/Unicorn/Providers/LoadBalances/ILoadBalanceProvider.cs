using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;
using Unicorn.Options;

namespace Unicorn.Providers.LoadBalances
{
    public interface ILoadBalanceProvider: IProvider
    {
        Task<Service> ExecuteAsync(
            IEnumerable<Service> services,
            Dictionary<string, LoadBalanceData> data,
            CancellationToken token = default);
    }
}
