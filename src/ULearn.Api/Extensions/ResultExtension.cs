
using Microsoft.AspNetCore.Mvc;
using ULearn.Domain.Enums;
using ULearn.Domain.Shared;

namespace ULearn.Api.Extensions;

public static class ResultExtensions
{

    #region Async
    public static async Task<IActionResult> ToActionResult(this Task<Result> resultTask)
    {
        var result = await resultTask;

        if (result.IsFailure)
        {
            return FailureResponse(result);
        }

        return new OkResult();
    }


    public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        if (result.IsFailure)
        {
            return FailureResponse(result);
        }

        return new OkObjectResult(result.Value);
    }
    #endregion

    #region Non-async
    // non-async

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsFailure)
        {
            return FailureResponse(result);
        }

        return new OkResult();
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsFailure)
        {
            return FailureResponse(result);
        }

        return new OkObjectResult(result.Value);
    }
    #endregion


    private static ObjectResult FailureResponse(Result result)
    {
        return result.Error.Code switch
        {
            ErroCodeEnum.BadRequest => new BadRequestObjectResult(result.Error),
            ErroCodeEnum.Unauthorized => new UnauthorizedObjectResult(result.Error),
            ErroCodeEnum.Forbidden => new ObjectResult(result.Error) { StatusCode = 403 },
            ErroCodeEnum.NotFound => new NotFoundObjectResult(result.Error),
            ErroCodeEnum.InternalServerError => new ObjectResult(new { status = 500, message = "Something went wrong." }) { StatusCode = 500 },
            _ => new ObjectResult(new { status = 500, message = "Something went wrong." }) { StatusCode = 500 }
        };
    }
}


