using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Datas;

namespace Unicorn.Providers.LoadBalances
{
    [ProviderName(ProviderName)]
    public class RandomLoadBalanceProvider : ILoadBalanceProvider
    {
        public const string ProviderName = "Random";
        public string Name => ProviderName;

        public async Task<Service> ExecuteAsync(
            IEnumerable<Service> services,
            Dictionary<string, LoadBalanceData> data,
            CancellationToken token = default)
        {
            if(services == null || services.Count() == 0)
            {
                return null;
            }
            var index = new Random().Next(0, services.Count());
            await Task.CompletedTask;
            return services.ElementAt(index);
        }
    }
}
