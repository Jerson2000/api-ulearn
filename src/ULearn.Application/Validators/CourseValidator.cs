

using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.DTOs;

namespace ULearn.Application.Validators;


public class CreateCourseRequestDtoValidator : AbstractValidator<CreateCourseRequestDto>
{
    public CreateCourseRequestDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title field cannot be empty");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Price field cannot be empty");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Missing category id");
    }
}



#region Extension
public static class CourseValidatorExtension
{
    public static IServiceCollection AddCourseValidator(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IValidator<CreateCourseRequestDto>, CreateCourseRequestDtoValidator>();
        return services;
    }
}
#endregion