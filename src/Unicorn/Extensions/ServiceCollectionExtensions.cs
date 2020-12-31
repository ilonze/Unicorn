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
            if (optionsSetup != null)
            {
                services.Configure(optionsSetup);
            }
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
