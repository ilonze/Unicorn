using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicorn.Options;

namespace Unicorn.LoadBalanceProviders
{
    public interface ILoadBalanceProvider: IProvider
    {
        Task<string> ExecuteAsync(IEnumerable<Service> services);
    }
}
