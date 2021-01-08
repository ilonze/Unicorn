using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.LoadBalanceProviders
{
    public class LeastConnectionLoadBalanceProvider : ILoadBalanceProvider
    {
        public const string ProviderName = "LeastConnection";
        public string Name => ProviderName;

        public Task<string> ExecuteAsync(IEnumerable<Service> services)
        {
            throw new NotImplementedException();
        }
    }
}
