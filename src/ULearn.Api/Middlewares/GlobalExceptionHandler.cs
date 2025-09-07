
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ULearn.Domain.Exceptions;

namespace ULearn.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private class Response { public int Code { get; set; } public string? Message { get; set; } };

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var resObj = new Response { Code = 500, Message = "Something went wrong!"};
            if (exception is AntiforgeryValidationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                resObj.Code = 400;
                resObj.Message = "Invalid Token";
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            }
            await httpContext.Response.WriteAsJsonAsync(resObj, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
