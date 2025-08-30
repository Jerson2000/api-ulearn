using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Domain.Interfaces.Repository;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using ULearn.Infrastructure.Data;
using ULearn.Infrastructure.Data.Repositories;
using ULearn.Infrastructure.Services;
using ULearn.Infrastructure.Settings;

namespace ULearn.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ULearnDbContext>(option =>
        {
            option.UseSqlServer(EnvironmentValues.DB_CONNECTION);
        });

        services.Configure<JwtSettings>(options =>
        {
            options.Issuer = "ULearn";
            options.Audience = "ULearnClient";
            options.AccessTokenExpiryMinutes = 30;
            options.Key = EnvironmentValues.JWT_KEY;
        });

        services.AddTransient<ICacheService, CacheService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}