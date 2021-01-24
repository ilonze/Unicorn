﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Unicorn.Providers.LoadBalances
{
    [ProviderName(ProviderName)]
    public class RandomLoadBalanceProvider : ILoadBalanceProvider
    {
        public const string ProviderName = "Random";
        public string Name => ProviderName;

        public Task<string> ExecuteAsync(IEnumerable<Service> services, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
