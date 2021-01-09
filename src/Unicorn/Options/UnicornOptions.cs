using System;
using System.Collections.Generic;
using System.Text;
using Unicorn.AggregateProviders;
using Unicorn.DataFormatConverters;
using Unicorn.EncryptProviders;
using Unicorn.FileProviders;
using Unicorn.LoadBalanceProviders;
using Unicorn.RateLimitProviders;
using Unicorn.Serializers;
using Unicorn.SignProviders;

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
        public Dictionary<string, Type> JsonSerializers { get; set; } = new Dictionary<string, Type>();


        public void AddAggregateProvider<T>(string name) where T: IAggregateProvider
        {
            AggregateProviders[name] = typeof(T);
        }
        public void AddLoadBalanceProvider<T>(string name) where T: ILoadBalanceProvider
        {
            LoadBalanceProviders[name] = typeof(T);
        }
        public void AddRateLimitProvider<T>(string name) where T: IRateLimitProvider
        {
            RateLimitProviders[name] = typeof(T);
        }
        public void AddFileProvider<T>(string name) where T: IFileProvider
        {
            FileProviders[name] = typeof(T);
        }
        public void AddSignProvider<T>(string name) where T: ISignProvider
        {
            SignProviders[name] = typeof(T);
        }
        public void AddEncryptProvider<T>(string name) where T : IEncryptProvider
        {
            EncryptProviders[name] = typeof(T);
        }
        public void AddDataFormatProvider<T>(string name) where T : IDataFormatProvider
        {
            DataFormatProviders[name] = typeof(T);
        }
        public void AddJsonSerializer<T>(string name) where T : IJsonSerializer
        {
            JsonSerializers[name] = typeof(T);
        }
    }
}
