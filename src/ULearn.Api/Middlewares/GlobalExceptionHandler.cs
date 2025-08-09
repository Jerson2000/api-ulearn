
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ULearn.Domain.Exceptions;

namespace ULearn.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails();

            if (exception is HttpException httpException)
            {
                httpContext.Response.StatusCode = httpException.StatusCode;
                problemDetails.Status = httpException.StatusCode;
                problemDetails.Title = httpException.Message;

                if (httpException is InternalServerException)
                {
                    _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                }
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Something went wrong.";

                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
