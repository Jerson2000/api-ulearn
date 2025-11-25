

using System.Data;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.DTOs;

namespace ULearn.Application.Validators;

public sealed class CreateQuestionRequestDtoValidator : AbstractValidator<CreateQuestionRequesDto>
{
    public CreateQuestionRequestDtoValidator()
    {
        RuleFor(x=>x.Question).NotEmpty().WithMessage("Question field cannot be empty");
        RuleFor(x => x.QuestionType).NotNull().WithMessage("QuestionType field must be provided").IsInEnum().WithMessage("QuestionType must be a valid value.");
        RuleFor(x=>x.OrderIndex).NotEmpty().WithMessage("OrderIndex field cannot be empty");
        RuleFor(x=>x.Points).NotEmpty().WithMessage("Points field cannot be empty");
    }
}


public sealed class CreateQuestionOptionRequestDtoValidator : AbstractValidator<CreateQuestionOptionRequestDto>
{
    public CreateQuestionOptionRequestDtoValidator()
    {
        RuleFor(x=>x.OptionText).NotEmpty().WithMessage("OptionText field cannot be empty");
        RuleFor(x=>x.IsCorrect).NotEmpty().WithMessage("IsCorrect field cannot be empty");       
    }
}



#region Extension
public static class QuestionValidatorExtension
{
    public static IServiceCollection AddQuestionValidator(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateQuestionRequesDto>, CreateQuestionRequestDtoValidator>();
        services.AddScoped<IValidator<CreateQuestionOptionRequestDto>, CreateQuestionOptionRequestDtoValidator>();
        return services;
    }
}
#endregion