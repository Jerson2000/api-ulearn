
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ULearn.Domain.Exceptions;
using ULearn.Domain.Shared;

namespace ULearn.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var resObj = new Error(Domain.Enums.ErroCodeEnum.InternalServerError, "Something went wrong!");
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(resObj, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
