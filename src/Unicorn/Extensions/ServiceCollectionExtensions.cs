using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Unicorn.Datas;
using Unicorn.Managers.Caches;
using Unicorn.Options;
using Unicorn.Providers.Aggregates;
using Unicorn.Providers.DataFormats;
using Unicorn.Providers.Encrypts;
using Unicorn.Providers.Files;
using Unicorn.Providers.LoadBalances;
using Unicorn.Providers.RateLimits;
using Unicorn.Providers.Signs;

namespace Unicorn.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnicorn(this IServiceCollection services, Action<UnicornOptions> optionsSetup = null)
        {
            services.AddCore()
                .AddSingleton<IUnicornCacheManager, UnicornCacheManager>();

            services.AddErrorResponseDataModel<ErrorResponseData>();

            services.AddAggregateProviders();
            services.AddDataFormatProviders();
            services.AddEncryptProviders();
            services.AddFileProviders();
            services.AddLoadBalanceProviders();
            services.AddRateLimitProviders();
            services.AddSignProviders();

            services.Configure<UnicornOptions>(options =>
            {
                optionsSetup?.Invoke(options);
                foreach (var routeRule in options.RouteRules)
                {
                    routeRule.RequestOptions ??= options.GlobalOptions.RequestOptions;
                    routeRule.ResponseOptions ??= options.GlobalOptions.ResponseOptions;
                    routeRule.ABListOptions ??= options.GlobalOptions.ABListOptions;
                    routeRule.AggregateOptions ??= options.GlobalOptions.AggregateOptions;
                    routeRule.AntiResubmitOptions ??= options.GlobalOptions.AntiResubmitOptions;
                    routeRule.AuthenticationOptions ??= options.GlobalOptions.AuthenticationOptions;
                    routeRule.CacheOptions ??= options.GlobalOptions.CacheOptions;
                    routeRule.CorsOptions ??= options.GlobalOptions.CorsOptions;
                    routeRule.HttpHandlerOptions ??= options.GlobalOptions.HttpHandlerOptions;
                    routeRule.LoadBalancerOptions ??= options.GlobalOptions.LoadBalancerOptions;
                    routeRule.QoSOptions ??= options.GlobalOptions.QoSOptions;
                    routeRule.RateLimitOptions ??= options.GlobalOptions.RateLimitOptions;
                    routeRule.SignOptions ??= options.GlobalOptions.SignOptions;
                    routeRule.EncryptOptions ??= options.GlobalOptions.EncryptOptions;
                    routeRule.DataFormatOptions ??= options.GlobalOptions.DataFormatOptions;
                }
            });

            services.AddRouting();

            return services;
        }

        public static IServiceCollection AddErrorResponseDataModel<TModel>(this IServiceCollection services)
            where TModel: class, IErrorResponseData
        {
            var descriptor = services.FirstOrDefault(r => r.ServiceType == typeof(IEncryptProvider));
            if(descriptor != null)
            {
                services.Remove(descriptor);
            }
            return services.AddTransient(typeof(IEncryptProvider), typeof(TModel));
        }

        public static IServiceCollection AddUnicorn(this IServiceCollection services, IConfiguration configuration, Action<UnicornOptions> optionsSetup = null)
        {
            return services.Configure<UnicornOptions>(configuration)
                .AddUnicorn(optionsSetup);
        }

        public static IServiceCollection AddAggregateProvider<TAggregateProvider>(this IServiceCollection services)
            where TAggregateProvider: class, IAggregateProvider
        {
            return services.AddScoped<TAggregateProvider>()
                .Configure<UnicornOptions>(options =>
                {
                    options.AddAggregateProvider<TAggregateProvider>();
                });
        }

        public static IServiceCollection AddAggregateProviders(this IServiceCollection services)
        {
            return services.AddAggregateProvider<DefaultAggregateProvider>()
                .AddAggregateProvider<ByteAggregateProvider>()
                .AddAggregateProvider<JsonMerginAggregateProvider>()
                .AddAggregateProvider<TextAggregateProvider>();
        }

        public static IServiceCollection AddDataFormatProvider<TDataFormatProvider>(this IServiceCollection services)
            where TDataFormatProvider: class, IDataFormatProvider
        {
            return services.AddScoped<TDataFormatProvider>()
                .Configure<UnicornOptions>(options =>
                {
                    options.AddDataFormatProvider<TDataFormatProvider>();
                });
        }

        public static IServiceCollection AddDataFormatProviders(this IServiceCollection services)
        {
            return services.AddDataFormatProvider<Base64ToBytesDataFormatProvider>()
                .AddDataFormatProvider<BytesToBase64DataFormatProvider>()
                .AddDataFormatProvider<JsonpToJsonDataFormatProvider>()
                .AddDataFormatProvider<JsonToJsonpDataFormatProvider>()
                .AddDataFormatProvider<JsonToXmlDataFormatProvider>()
                .AddDataFormatProvider<XmlToJsonDataFormatProvider>()
                .AddDataFormatProvider<YamlToJsonDataFormatProvider>()
                .AddDataFormatProvider<JsonToYamlDataFormatProvider>()
                .AddDataFormatProvider<XmlToYamlDataFormatProvider>()
                .AddDataFormatProvider<YamlToXmlDataFormatProvider>();
        }

        public static IServiceCollection AddEncryptProvider<TEncryptProvider>(this IServiceCollection services)
            where TEncryptProvider: class, IEncryptProvider
        {
            return services.AddScoped<TEncryptProvider>()
                .Configure<UnicornOptions>(options =>
                {
                    options.AddEncryptProvider<TEncryptProvider>();
                });
        }

        public static IServiceCollection AddEncryptProviders(this IServiceCollection services)
        {
            return services.AddEncryptProvider<AesEncryptProvider>()
                .AddEncryptProvider<DesEncryptProvider>()
                .AddEncryptProvider<RsaEncryptProvider>();
        }

        public static IServiceCollection AddFileProvider<TFileProvider>(this IServiceCollection services)
            where TFileProvider: class, IFileProvider
        {
            return services.AddScoped<TFileProvider>()
                .Configure<UnicornOptions>(options =>
                {
                    options.AddFileProvider<TFileProvider>();
                });
        }

        public static IServiceCollection AddFileProviders(this IServiceCollection services)
        {
            return services.AddFileProvider<StaticFileProvider>()
                .AddFileProvider<VirtualFileProvider>();
        }

        public static IServiceCollection AddLoadBalanceProvider<TLoadBalanceProvider>(this IServiceCollection services)
            where TLoadBalanceProvider: class, ILoadBalanceProvider
        {
            return services.AddScoped<TLoadBalanceProvider>()
                .Configure<UnicornOptions>(options =>
                {
                    options.AddLoadBalanceProvider<TLoadBalanceProvider>();
                });
        }

        public static IServiceCollection AddLoadBalanceProviders(this IServiceCollection services)
        {
            return services.AddLoadBalanceProvider<LeastConnectionLoadBalanceProvider>()
                .AddLoadBalanceProvider<RandomLoadBalanceProvider>()
                .AddLoadBalanceProvider<RoundRobinLoadBalanceProvider>();
        }

        public static IServiceCollection AddRateLimitProvider<TRateLimitProvider>(this IServiceCollection services)
            where TRateLimitProvider: class, IRateLimitProvider
        {
            return services.AddScoped<TRateLimitProvider>()
                .Configure<UnicornOptions>(options =>
                {
                    options.AddRateLimitProvider<TRateLimitProvider>();
                });
        }

        public static IServiceCollection AddRateLimitProviders(this IServiceCollection services)
        {
            return services.AddRateLimitProvider<CookieRateLimitProvider>()
                .AddRateLimitProvider<GeneralRateLimitProvider>()
                .AddRateLimitProvider<HeaderRateLimitProvider>()
                .AddRateLimitProvider<IpRateLimitProvider>()
                .AddRateLimitProvider<UserAgentRateLimitProvider>();
        }

        public static IServiceCollection AddSignProvider<TSignProvider>(this IServiceCollection services)
            where TSignProvider: class, ISignProvider
        {
            return services.AddScoped<TSignProvider>()
                .Configure<UnicornOptions>(options =>
                {
                    options.AddSignProvider<TSignProvider>();
                });
        }

        public static IServiceCollection AddSignProviders(this IServiceCollection services)
        {
            return services.AddSignProvider<Md5SignProvider>()
                .AddSignProvider<HashSignProvider>()
                .AddSignProvider<RsaSignProvider>();
        }
    }
}
