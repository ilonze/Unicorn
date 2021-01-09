using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicorn.Providers.Aggregates;
using Unicorn.Providers.DataFormats;
using Unicorn.Providers.Encrypts;
using Unicorn.Providers.Files;
using Unicorn.Providers.LoadBalances;
using Unicorn.Providers.RateLimits;
using Unicorn.Providers.Signs;
using Unicorn.Serializers;

namespace Unicorn.Options
{
    public class UnicornOptions: IOptions
    {
        public bool IsMaster { get; set; }
        public bool UnicornDataUseDistributedCace { get; set; }
        public bool ResponseDataUseDistributedCace { get; set; }
        public bool AntiResubmitDataUseDistributedCace { get; set; }
        public List<RouteRule> RouteRules { get; set; } = new List<RouteRule>();
        public List<Service> Services { get; set; } = new List<Service>();
        public GlobalOptions GlobalOptions { get; set; }
        public bool IsShareLoadBalance { get; set; }
        public bool IsShareRateLimit { get; set; }
        public string CacheKeyPrefix { get; set; }

        public Dictionary<string, Type> AggregateProviders { get; set; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> LoadBalanceProviders { get; set; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> RateLimitProviders { get; set; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> FileProviders { get; set; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> SignProviders { get; set; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> EncryptProviders { get; set; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> DataFormatProviders { get; set; } = new Dictionary<string, Type>();


        public void AddAggregateProvider<T>() where T: IAggregateProvider
        {
            AddProvider<T>(AggregateProviders);
        }
        public void AddLoadBalanceProvider<T>() where T: ILoadBalanceProvider
        {
            AddProvider<T>(LoadBalanceProviders);
        }
        public void AddRateLimitProvider<T>() where T: IRateLimitProvider
        {
            AddProvider<T>(RateLimitProviders);
        }
        public void AddFileProvider<T>() where T: IFileProvider
        {
            AddProvider<T>(FileProviders);
        }
        public void AddSignProvider<T>() where T: ISignProvider
        {
            AddProvider<T>(SignProviders);
        }
        public void AddEncryptProvider<T>() where T : IEncryptProvider
        {
            AddProvider<T>(EncryptProviders);
        }
        public void AddDataFormatProvider<T>() where T : IDataFormatProvider
        {
            AddProvider<T>(DataFormatProviders);
        }

        private void AddProvider<T>(Dictionary<string, Type> providers)
            where T: IProvider
        {
            var type = typeof(T);
            var name = type.GetCustomAttributes(false).OfType<ProviderNameAttribute>().FirstOrDefault()?.Name;
            providers[name] = type;
        }
    }
}
