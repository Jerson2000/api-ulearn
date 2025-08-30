
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using ULearn.Domain.Shared;

namespace ULearn.Infrastructure.Utils;

public static class CacheHelper
{
    public static string SerializeToBase64<T>(T data)
    {
        var json = JsonConvert.SerializeObject(data);
        var bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public static T? DeserializeFromBase64<T>(string base64Data)
    {
        if (string.IsNullOrEmpty(base64Data)) return default;

        var bytes = Convert.FromBase64String(base64Data);
        var json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static string GenerateCacheKey<TEntity>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Cache key must be non-null and non-empty.", nameof(key));

        var typeName = typeof(TEntity).Name;
        return $"{typeName}:{key}";
    }

    public static async Task<T> GetOrSetCacheAsync<T>(
        string cacheKey,
        IMemoryCache memCache,
        IDistributedCache distributedCache,
        Func<Task<T>> getDataAsync,
        TimeSpan? cacheDuration = null)
    {
        if (string.IsNullOrWhiteSpace(cacheKey))
            throw new ArgumentException("Cache key must be non-empty.", nameof(cacheKey));

        return EnvironmentValues.IS_REDIS_CACHED_ENABLE
            ? await GetOrSetDistributedCacheAsync(cacheKey, distributedCache, getDataAsync, cacheDuration)
            : await GetOrSetMemoryCacheAsync(cacheKey, memCache, getDataAsync, cacheDuration);
    }

    private static async Task<T> GetOrSetDistributedCacheAsync<T>(
        string cacheKey,
        IDistributedCache distributedCache,
        Func<Task<T>> getDataAsync,
        TimeSpan? cacheDuration)
    {
        var cachedData = await distributedCache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            var deserialized = JsonConvert.DeserializeObject<T>(cachedData);
            if (deserialized != null)
                return deserialized;
        }

        var data = await getDataAsync();
        if (data != null)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            var options = new DistributedCacheEntryOptions();

            if (cacheDuration.HasValue)
                options.SetAbsoluteExpiration(cacheDuration.Value);

            await distributedCache.SetStringAsync(cacheKey, serializedData, options);
        }

        return data!;
    }

    private static async Task<T> GetOrSetMemoryCacheAsync<T>(
        string cacheKey,
        IMemoryCache memCache,
        Func<Task<T>> getDataAsync,
        TimeSpan? cacheDuration)
    {
        if (memCache.TryGetValue(cacheKey, out T? cachedValue))
            return cachedValue!;

        var data = await getDataAsync();
        if (data != null)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            var options = new MemoryCacheEntryOptions();

            if (cacheDuration.HasValue)
                options.SetAbsoluteExpiration(cacheDuration.Value);

            options.SetSize(serializedData.Length);
            
            memCache.Set(cacheKey, data, options);
        }

        return data!;
    }
}
