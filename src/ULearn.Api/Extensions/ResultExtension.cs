using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using ULearn.Api.Utils;
using ULearn.Domain.Shared;

namespace ULearn.Api.Extensions;

public static class ResultExtensions
{

    #region Async
    public static async Task<IActionResult> ToActionResult(this Task<Result> resultTask)
    {
        var result = await resultTask;

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        return new OkResult();
    }


    public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        return new OkObjectResult(result.Value);
    }
    #endregion

    #region Non-async
    // non-async

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        return new OkResult();
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        return new OkObjectResult(result.Value);
    }
    #endregion

    #region Cache MemoryCache and DistributedCache

    /// <summary>
    /// Executes a result-producing task and caches the successful result in in-memory cache (IMemoryCache).
    /// If the value is already cached, it returns the cached value immediately.
    /// If the task fails, it returns an appropriate error response (404, 400, etc.).
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="resultTask">A Task that returns a Result&lt;T&gt;.</param>
    /// <param name="cache">The IMemoryCache instance to use for caching.</param>
    /// <param name="cacheKey">A unique key used to retrieve/store the cached item.</param>
    /// <param name="cacheOptions">Optional cache entry options (expiration, size, etc.).</param>
    /// <param name="onSuccess">Optional callback to execute on a successful result (after caching).</param>
    /// <returns>An IActionResult representing the cached or fresh result, or an error response.</returns>
    public static async Task<IActionResult> ToMemoryCachedActionResult<T>(
        this Task<Result<T>> resultTask,
        IMemoryCache cache,
        string cacheKey,
        MemoryCacheEntryOptions? cacheOptions = null,
        Func<T, Task>? onSuccess = null)
    {
        if (cache.TryGetValue(cacheKey, out string? cachedBase64) && cachedBase64 is not null)
        {
            var cachedValue = CacheHelper.DeserializeFromBase64<T>(cachedBase64);
            if (cachedValue is not null)
                return new OkObjectResult(cachedValue);
        }

        var result = await resultTask;

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        // Set default cache options if none provided
        cacheOptions ??= new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetSize(1);

        // Serialize value and store in cache
        var serializedValue = CacheHelper.SerializeToBase64(result.Value!);
        cache.Set(cacheKey, serializedValue, cacheOptions);

        if (onSuccess is not null)
            await onSuccess(result.Value);

        return new OkObjectResult(result.Value);
    }





    /// <summary>
    /// Executes a result-producing task and caches the successful result in distributed cache (IDistributedCache).
    /// If the value is already cached, it deserializes and returns it immediately.
    /// If the task fails, it returns an appropriate error response (404, 400, etc.).
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="resultTask">A Task that returns a Result&lt;T&gt;.</param>
    /// <param name="cache">The IDistributedCache instance to use for caching (e.g., Redis).</param>
    /// <param name="cacheKey">A unique key used to retrieve/store the cached item.</param>
    /// <param name="cacheOptions">Optional distributed cache entry options (absolute expiration, etc.).</param>
    /// <param name="onSuccess">Optional callback to execute on a successful result (after caching).</param>
    /// <returns>An IActionResult representing the cached or fresh result, or an error response.</returns>
    public static async Task<IActionResult> ToDistributedCachedActionResult<T>(
        this Task<Result<T>> resultTask,
        IDistributedCache cache,
        string cacheKey,
        DistributedCacheEntryOptions? cacheOptions = null,
        Func<T, Task>? onSuccess = null)
    {
        var cachedData = await cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedValue = CacheHelper.DeserializeFromBase64<T>(cachedData);
            return new OkObjectResult(cachedValue);
        }

        var result = await resultTask;

        // Handle failure
        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        var serialized = CacheHelper.SerializeToBase64(result.Value);
        cacheOptions ??= new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        };

        await cache.SetStringAsync(cacheKey, serialized, cacheOptions);

        if (onSuccess is not null)
            await onSuccess(result.Value);

        return new OkObjectResult(result.Value);
    }


    #endregion

}


