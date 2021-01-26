using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.LoadBalances
{
    [ProviderName(ProviderName)]
    public class RoundRobinLoadBalanceProvider : ILoadBalanceProvider
    {
        public const string ProviderName = "RoundRobin";
        public string Name => ProviderName;

        public async Task<Service> ExecuteAsync(
            IEnumerable<Service> services,
            Dictionary<string, LoadBalanceData> data,
            CancellationToken token = default)
        {
            if (services == null || services.Count() == 0)
            {
                return null;
            }
            var useTime = data.Values.Min(r => r.LastUseTime);
            var serviceId = data.First(r => r.Value.LastUseTime == useTime).Key;

            await Task.CompletedTask;
            return services.FirstOrDefault(r => r.Id == serviceId);
        }
    }
}
