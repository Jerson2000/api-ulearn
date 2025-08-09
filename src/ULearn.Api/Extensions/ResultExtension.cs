using Microsoft.AspNetCore.Mvc;
using ULearn.Domain.Shared;

namespace ULearn.Api.Extensions;

public static class ResultExtensions
{
    public static async Task<IActionResult> ToActionResult(this Task<Result> resultTask)
    {
        var result = await resultTask;

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        return new OkResult();
    }


    public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        return new OkObjectResult(result.Value);
    }


    // traditional

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        return new OkResult();
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                404 => new NotFoundObjectResult(result.Error),
                400 => new BadRequestObjectResult(result.Error),
                403 => new ObjectResult(result.Error) { StatusCode = 403 },
                _ => new ObjectResult(new { code = 500, message = "Something went wrong." }) { StatusCode = 500 }
            };
        }

        return new OkObjectResult(result.Value);
    }
}


