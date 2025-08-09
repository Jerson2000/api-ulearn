using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Domain.Interfaces.Repository;
using ULearn.Domain.Interfaces.Services;
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
            option.UseSqlServer(config.GetConnectionString("Default"));
        });

        services.Configure<JwtSettings>(config.GetSection("JwtSettings"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}