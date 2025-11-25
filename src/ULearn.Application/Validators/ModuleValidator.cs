

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.DTOs;

namespace ULearn.Application.Validators;

public sealed class CreateModuleRequestDtoValidator : AbstractValidator<CreateModuleRequestDto>
{
    public CreateModuleRequestDtoValidator()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Title field cannot be empty");
        RuleFor(x=>x.OrderIndex).NotEmpty().WithMessage("OrderIndex field cannot be empty");
    }
}


#region Extension
public static class ModuleValidatorExtension
{
    public static IServiceCollection AddModuleValidator(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateModuleRequestDto>, CreateModuleRequestDtoValidator>();
        return services;
    }
}
#endregion