using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.Interfaces;
using ULearn.Application.Services;

namespace ULearn.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStorageAppService, StorageService>();

        // Validator
        services.AddValidatorsRegistration(config);
        return services;
    }
}
