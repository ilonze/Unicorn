using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.Providers.LoadBalances
{
    [ProviderName(ProviderName)]
    public class NoneLoadBalanceProvider : ILoadBalanceProvider
    {
        public const string ProviderName = "None";
        public string Name => ProviderName;

        public Task<string> ExecuteAsync(IEnumerable<Service> services)
        {
            throw new NotImplementedException();
        }
    }
}
