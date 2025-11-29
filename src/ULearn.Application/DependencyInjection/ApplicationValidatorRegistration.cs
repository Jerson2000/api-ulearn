


using FluentValidation;
using Microsoft.AspNetCore.Http;
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

        // File Validator
        services.AddScoped<IValidator<UploadFileRequestDto>, UploadFileRequestDtoValidator>();
        services.AddScoped<IValidator<UploadFilesRequestDto>, UploadFilesRequestDtoValidator>();
        services.AddScoped<IValidator<IFormFile>, IFormFileValidator>();
        
        services.AddCourseValidator(config);
        services.AddLessonValidator();
        services.AddModuleValidator();
        services.AddQuizValidator();
        services.AddQuestionValidator();
        services.AddAssignmentValidator();

        return services;
    }
}