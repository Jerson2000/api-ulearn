


namespace ULearn.Domain.Interfaces.Services;

public interface ICacheService
{

    Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? duration = null);
    Task RemoveCacheAsync(string cacheKey);
}