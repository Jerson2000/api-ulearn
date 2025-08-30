

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using ULearn.Domain.Interfaces.Services;
using ULearn.Infrastructure.Utils;

namespace ULearn.Infrastructure.Services;

public class CacheService(IMemoryCache memoryCache, IDistributedCache distributedCache) : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? duration = null)
    {
        return await CacheHelper.GetOrSetCacheAsync(key, _memoryCache, _distributedCache, factory, duration);
    }
}