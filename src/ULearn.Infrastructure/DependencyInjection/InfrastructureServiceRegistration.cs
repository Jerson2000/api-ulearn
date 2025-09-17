using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Domain.Interfaces.Repository;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using ULearn.Infrastructure.Configurations;
using ULearn.Infrastructure.Data;
using ULearn.Infrastructure.Data.Repositories;
using ULearn.Infrastructure.Services;

namespace ULearn.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ULearnDbContext>(option =>
        {
            option.UseSqlServer(EnvironmentValues.DB_CONNECTION);
        });

        services.AddJWTConfig(config);
        services.AddCacheConfig();
        services.AddRateLimitConfig(config);
        services.AddEmailConfig();

        services.AddTransient<ICacheService, CacheService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}