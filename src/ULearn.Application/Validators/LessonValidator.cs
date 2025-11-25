

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.DTOs;

namespace ULearn.Application.Validators;

public sealed class CreateLessonRequestDtoValidator : AbstractValidator<CreateLessonRequestDto>
{
    public CreateLessonRequestDtoValidator()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Title field cannot be empty");
        RuleFor(x=>x.ContentType).NotEmpty().WithMessage("ContentType field cannot be empty").IsInEnum().WithMessage("ContentType must be a valid enum value");  
        RuleFor(x=>x.OrderIndex).NotEmpty().WithMessage("OrderIndex field cannot be empty");
        RuleFor(x=>x.IsPreview).NotEmpty().WithMessage("IsPreview field cannot be empty");
    }
}


#region Extension
public static class LessonValidatorExtension
{
    public static IServiceCollection AddLessonValidator(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateLessonRequestDto>, CreateLessonRequestDtoValidator>();
        return services;
    }
}
#endregion