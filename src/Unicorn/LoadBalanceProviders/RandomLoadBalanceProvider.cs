using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Core;

namespace Unicorn.LoadBalanceProviders
{
    public class RandomLoadBalanceProvider : ILoadBalanceProvider
    {
        public const string ProviderName = "Random";
        public string Name => ProviderName;

        public Task<string> ExecuteAsync(IEnumerable<Service> services)
        {
            throw new NotImplementedException();
        }
    }
}
