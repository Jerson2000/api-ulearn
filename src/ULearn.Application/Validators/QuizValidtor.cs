

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.DTOs;

namespace ULearn.Application.Validators;

public sealed class CreateQuizRequestDtoValidator : AbstractValidator<CreateQuizRequestDto>
{
    public CreateQuizRequestDtoValidator()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Title field cannot be empty");
        RuleFor(x=>x.PassingScore).NotEmpty().WithMessage("PassingScore field cannot be empty");
        RuleFor(x=>x.TimeLimitInMinutes).NotEmpty().WithMessage("TimeLimitInMinutes field cannot be empty");        
    }
}




#region Extension
public static class QuizValidatorExtension
{
    public static IServiceCollection AddQuizValidator(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateQuizRequestDto>, CreateQuizRequestDtoValidator>();
        return services;
    }
}
#endregion