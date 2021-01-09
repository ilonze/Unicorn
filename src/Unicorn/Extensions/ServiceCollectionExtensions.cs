using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
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
            services.AddCore();

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
                }
            });

            services.AddRouting();

            return services;
        }

        public static IServiceCollection AddUnicorn(this IServiceCollection services, IConfiguration configuration, Action<UnicornOptions> optionsSetup = null)
        {
            services.Configure<UnicornOptions>(configuration);
            return services.AddUnicorn(optionsSetup);
        }

        public static IServiceCollection AddAggregateProviders(this IServiceCollection services)
        {
            services.AddScoped<DefaultAggregateProvider>();
            services.AddScoped<ByteAggregateProvider>();
            services.AddScoped<JsonMerginAggregateProvider>();
            services.AddScoped<TextAggregateProvider>();

            services.Configure<UnicornOptions>(options =>
            {
                options.AddAggregateProvider<DefaultAggregateProvider>();
                options.AddAggregateProvider<ByteAggregateProvider>();
                options.AddAggregateProvider<JsonMerginAggregateProvider>();
                options.AddAggregateProvider<TextAggregateProvider>();
            });
            return services;
        }

        public static IServiceCollection AddDataFormatProviders(this IServiceCollection services)
        {
            services.AddScoped<Base64ToBytesDataFormatProvider>();
            services.AddScoped<BytesToBase64DataFormatProvider>();
            services.AddScoped<JsonpToJsonDataFormatProvider>();
            services.AddScoped<JsonToJsonpDataFormatProvider>();
            services.AddScoped<JsonToXmlDataFormatProvider>();
            services.AddScoped<XmlToJsonDataFormatProvider>();
            services.AddScoped<YamlToJsonDataFormatProvider>();
            services.AddScoped<JsonToYamlDataFormatProvider>();
            services.AddScoped<XmlToYamlDataFormatProvider>();
            services.AddScoped<YamlToXmlDataFormatProvider>();

            services.Configure<UnicornOptions>(options =>
            {
                options.AddDataFormatProvider<Base64ToBytesDataFormatProvider>();
                options.AddDataFormatProvider<BytesToBase64DataFormatProvider>();
                options.AddDataFormatProvider<JsonpToJsonDataFormatProvider>();
                options.AddDataFormatProvider<JsonToJsonpDataFormatProvider>();
                options.AddDataFormatProvider<JsonToXmlDataFormatProvider>();
                options.AddDataFormatProvider<XmlToJsonDataFormatProvider>();
                options.AddDataFormatProvider<YamlToJsonDataFormatProvider>();
                options.AddDataFormatProvider<JsonToYamlDataFormatProvider>();
                options.AddDataFormatProvider<XmlToYamlDataFormatProvider>();
                options.AddDataFormatProvider<YamlToXmlDataFormatProvider>();
            });
            return services;
        }

        public static IServiceCollection AddEncryptProviders(this IServiceCollection services)
        {
            services.AddScoped<AesEncryptProvider>();
            services.AddScoped<DesEncryptProvider>();
            services.AddScoped<RsaEncryptProvider>();

            services.Configure<UnicornOptions>(options =>
            {
                options.AddEncryptProvider<AesEncryptProvider>();
                options.AddEncryptProvider<DesEncryptProvider>();
                options.AddEncryptProvider<RsaEncryptProvider>();
            });
            return services;
        }

        public static IServiceCollection AddFileProviders(this IServiceCollection services)
        {
            services.AddScoped<StaticFileProvider>();
            services.AddScoped<VirtualFileProvider>();

            services.Configure<UnicornOptions>(options =>
            {
                options.AddFileProvider<StaticFileProvider>();
                options.AddFileProvider<VirtualFileProvider>();
            });
            return services;
        }

        public static IServiceCollection AddLoadBalanceProviders(this IServiceCollection services)
        {
            services.AddScoped<NoneLoadBalanceProvider>();
            services.AddScoped<LeastConnectionLoadBalanceProvider>();
            services.AddScoped<RandomLoadBalanceProvider>();
            services.AddScoped<RoundRobinLoadBalanceProvider>();

            services.Configure<UnicornOptions>(options =>
            {
                options.AddLoadBalanceProvider<NoneLoadBalanceProvider>();
                options.AddLoadBalanceProvider<LeastConnectionLoadBalanceProvider>();
                options.AddLoadBalanceProvider<RandomLoadBalanceProvider>();
                options.AddLoadBalanceProvider<RoundRobinLoadBalanceProvider>();
            });
            return services;
        }

        public static IServiceCollection AddRateLimitProviders(this IServiceCollection services)
        {
            services.AddScoped<CookieRateLimitProvider>();
            services.AddScoped<GeneralRateLimitProvider>();
            services.AddScoped<HeaderRateLimitProvider>();
            services.AddScoped<IpRateLimitProvider>();
            services.AddScoped<UserAgentRateLimitProvider>();

            services.Configure<UnicornOptions>(options =>
            {
                options.AddRateLimitProvider<CookieRateLimitProvider>();
                options.AddRateLimitProvider<GeneralRateLimitProvider>();
                options.AddRateLimitProvider<HeaderRateLimitProvider>();
                options.AddRateLimitProvider<IpRateLimitProvider>();
                options.AddRateLimitProvider<UserAgentRateLimitProvider>();
            });
            return services;
        }

        public static IServiceCollection AddSignProviders(this IServiceCollection services)
        {
            services.AddScoped<Md5SignProvider>();
            services.AddScoped<RsaSignProvider>();

            services.Configure<UnicornOptions>(options =>
            {
                options.AddSignProvider<Md5SignProvider>();
                options.AddSignProvider<RsaSignProvider>();
            });
            return services;
        }
    }
}
