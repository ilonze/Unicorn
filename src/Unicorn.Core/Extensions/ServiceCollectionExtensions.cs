﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Unicorn.Core.Caching;
using Unicorn.Core.Serializers;

namespace Unicorn.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.TryAddTransient<IJsonSerializer, JsonSerializer>();
            services.TryAddTransient<IDistributedCacheSerializer, DistributedCacheSerializer>();
            services.TryAddSingleton(typeof(IUnicornCacheProvider), typeof(MemoryUnicornCacheProvider));
            services.TryAddSingleton(typeof(IUnicornCacheProvider), typeof(DistributedUnicornCacheProvider));
            return services;
        }
    }
}