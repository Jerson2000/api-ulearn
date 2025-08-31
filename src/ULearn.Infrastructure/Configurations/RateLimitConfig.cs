using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ULearn.Infrastructure.Settings;

namespace ULearn.Infrastructure.Configurations;

public static class RateLimitConfig
{
    public static IServiceCollection AddRateLimitConfig(this IServiceCollection services, IConfiguration configuration)
    {        
        services.Configure<RateLimitSettings>(_=> { });
        services.AddRateLimiter(options =>
        {
            var settings = services.BuildServiceProvider()
                                   .GetRequiredService<IOptions<RateLimitSettings>>()
                                   .Value;

            // ----- Global Throttle: Chained by IP + User -----
            options.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                // Per IP limiter
                PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip",
                        _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = settings.IPPermitLimit,
                            Window = TimeSpan.FromSeconds(settings.IPWindowSeconds),
                            SegmentsPerWindow = settings.IPSegments,
                            QueueLimit = settings.IPQueueLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            AutoReplenishment = true
                        })),

                // Per Authenticated User limiter
                PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var user = httpContext.User.Identity?.Name;
                    return RateLimitPartition.GetTokenBucketLimiter(
                        user ?? "anonymous",
                        _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = settings.UserTokenLimit,
                            TokensPerPeriod = settings.UserTokensPerPeriod,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(settings.UserReplenishSeconds),
                            QueueLimit = settings.UserQueueLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            AutoReplenishment = true
                        });
                }));

            // ----- Policy for auth endpoints (e.g., login) -----
            options.AddFixedWindowLimiter("AuthPolicy", config =>
            {
                config.PermitLimit = settings.AuthPermitLimit;
                config.Window = TimeSpan.FromSeconds(settings.AuthWindowSeconds);
                config.QueueLimit = settings.AuthQueueLimit;
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            // ----- Unified rejection handling -----
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers["Retry-After"] = ((int)retryAfter.TotalSeconds).ToString();
                }

                var message = new
                {
                    Message = "Too many requests. Please try again later.",
                    Status = StatusCodes.Status429TooManyRequests
                };

                await context.HttpContext.Response.WriteAsJsonAsync(message, cancellationToken);
            };
        });

        return services;
    }
}