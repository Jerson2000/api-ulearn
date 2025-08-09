


using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.DTOs;
using ULearn.Application.Validators;

namespace ULearn.Application.DependencyInjection;

public static class ApplicationValidatorRegistration
{
    public static IServiceCollection AddValidatorsRegistration(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
        services.AddScoped<IValidator<LoginDto>, UserLoginDtoValidator>();
        return services;
    }
}