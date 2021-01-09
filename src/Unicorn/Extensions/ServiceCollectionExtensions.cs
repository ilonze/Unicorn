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

namespace Unicorn.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnicorn(this IServiceCollection services, Action<UnicornOptions> optionsSetup = null)
        {
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
    }
}
