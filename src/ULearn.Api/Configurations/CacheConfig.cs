

using System.Collections;
using StackExchange.Redis;

namespace ULearn.Api.Configurations;

public static class CacheConfig
{

    public static IServiceCollection AddCacheConfig(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Environment.GetEnvironmentVariable("REDIS_HOST"); // Redis Host:Port
            options.InstanceName = "RedisCache:";  // Optional, adds a prefix to cache keys
            options.ConfigurationOptions = new ConfigurationOptions
            {
                Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD")
            };
        });

        return services;
    }
}