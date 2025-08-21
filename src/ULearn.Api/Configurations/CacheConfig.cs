

using System.Collections;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using ULearn.Domain.Shared;

namespace ULearn.Api.Configurations;

public static class CacheConfig
{

    public static IServiceCollection AddCacheConfig(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.Configure<MemoryCacheOptions>(options =>
        {       
            options.SizeLimit = 1024;    
            options.CompactionPercentage = 0.15; 
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = "RedisCache:";
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { { EnvironmentValues.REDIS_HOST!, int.Parse(EnvironmentValues.REDIS_PORT ?? "6379") } },
                Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD"),
                AbortOnConnectFail = false,
                ConnectRetry = 5,
                ConnectTimeout = 5000,
                SyncTimeout = 5000,
                Ssl = true
            };
        });

        return services;
    }
}