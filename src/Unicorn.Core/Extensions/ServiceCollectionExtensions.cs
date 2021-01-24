using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Unicorn.Caching;
using Unicorn.Serializers;
using Unicorn.Serializers.Newtonsoft;

namespace Unicorn.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.TryAddScoped<IJsonSerializer, NewtonsoftJsonSerializer>();
            services.TryAddScoped<IDistributedCacheSerializer, DistributedCacheSerializer>();
            services.TryAddSingleton<MemoryUnicornCacheProvider>();
            services.TryAddSingleton<DistributedUnicornCacheProvider>();
            return services;
        }
    }
}
