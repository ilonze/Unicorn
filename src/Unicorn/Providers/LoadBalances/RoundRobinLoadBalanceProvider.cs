﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Providers.LoadBalances
{
    [ProviderName(ProviderName)]
    public class RoundRobinLoadBalanceProvider : ILoadBalanceProvider
    {
        public const string ProviderName = "RoundRobin";
        public string Name => ProviderName;

        public Task<string> ExecuteAsync(IEnumerable<Service> services)
        {
            throw new NotImplementedException();
        }
    }
}