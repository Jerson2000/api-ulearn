using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using ULearn.Application.Interfaces;

namespace ULearn.Api.Filters;

public class CustomValidationFilter : IAsyncActionFilter
{
    // public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    // {
    //     foreach (var argument in context.ActionArguments.Values)
    //     {
    //         if (argument is not IValidateRequest) continue;

    //         var validationResult = await ValidateAsync(argument, context.HttpContext.RequestServices);

    //         if (!validationResult.IsValid)
    //         {
    //             context.Result = CreateValidationProblemResult(validationResult);
    //             return;
    //         }
    //     }

    //     await next();
    // }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Check for ModelState validation errors first
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors?.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Title = "One or more validation errors occurred.",
                Status = (int)HttpStatusCode.BadRequest
            };

            context.Result = new BadRequestObjectResult(new { Message = "One or more validation errors occurred.", Status = 400, Errors = errors });
            return;
        }

        // Custom FluentValidation logic
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is not IValidateRequest) continue;

            var validationResult = await ValidateAsync(argument, context.HttpContext.RequestServices);

            if (!validationResult.IsValid)
            {
                context.Result = CreateValidationProblemResult(validationResult);
                return;
            }
        }

        await next();
    }


    private static async Task<ValidationResult> ValidateAsync(object model, IServiceProvider services)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());

        if (services.GetService(validatorType) is not IValidator validator)
            return new ValidationResult();

        var contextType = typeof(ValidationContext<>).MakeGenericType(model.GetType());
        var validationContext = (IValidationContext)Activator.CreateInstance(contextType, model)!;

        return await validator.ValidateAsync(validationContext);
    }

    private static IActionResult CreateValidationProblemResult(ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problemDetails = new ValidationProblemDetails(errors)
        {
            Title = "One or more validation errors occurred.",
            Status = (int)HttpStatusCode.BadRequest,
        };


        return new BadRequestObjectResult(problemDetails);
    }
}
