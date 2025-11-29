

using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.DTOs;

namespace ULearn.Application.Validators;


public class CreateAssignmentRequestDtoValidator : AbstractValidator<CreateAssignmentRequestDto>
{
    public CreateAssignmentRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty.");

        RuleFor(x => x.Instructions)
            .NotEmpty()
            .WithMessage("Instructions cannot be empty.");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("DueDate cannot be empty.")
            .Must(d => d > DateTime.UtcNow)
            .WithMessage("DueDate must be in the future.");

        RuleFor(x => x.MaxPoints)
            .GreaterThan(0)
            .WithMessage("MaxPoints must be greater than zero.");
    }
}


public class CreateAssignmentSubmissionRequestDtoValidator : AbstractValidator<CreateAssignmentSubmissionRequestDto>
{
    public CreateAssignmentSubmissionRequestDtoValidator()
    {
        RuleFor(x => x.FileUrl)
            .NotEmpty()
            .WithMessage("FileUrl cannot be empty.")
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("FileUrl must be a valid URL.");

        RuleFor(x => x.TextSubmission)
            .MaximumLength(5000)
            .WithMessage("TextSubmission cannot exceed 5000 characters.");
    }
}




#region Extension
public static class AssignmentValidatorExtension
{
    public static IServiceCollection AddAssignmentValidator(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateAssignmentRequestDto>, CreateAssignmentRequestDtoValidator>();
        services.AddScoped<IValidator<CreateAssignmentSubmissionRequestDto>, CreateAssignmentSubmissionRequestDtoValidator>();
        return services;
    }
}
#endregion