using Microsoft.AspNetCore.Antiforgery;
using ULearn.Domain.Shared;

namespace ULearn.Api.Middlewares
{
    public class AntiCSRFValidationMiddleware(RequestDelegate next, IAntiforgery antiforgery)
    {
        private readonly RequestDelegate _next = next;
        private readonly IAntiforgery _antiforgery = antiforgery;

        public async Task InvokeAsync(HttpContext context)
        {

            var excludedPaths = new[]
            {
                "/api/auth/login",
            };

            if (excludedPaths.Any(p => context.Request.Path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }
            if (!HttpMethods.IsGet(context.Request.Method) &&
                !HttpMethods.IsHead(context.Request.Method) &&
                !HttpMethods.IsOptions(context.Request.Method) &&
                !HttpMethods.IsTrace(context.Request.Method))
            {
                try
                {
                    await _antiforgery.ValidateRequestAsync(context);
                }
                catch (AntiforgeryValidationException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new Error(Domain.Enums.ErroCodeEnum.BadRequest, "Invalid token."));
                    return;
                }
            }

            await _next(context);
        }
    }

    public static class AntiCSRFValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiforgeryValidation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AntiCSRFValidationMiddleware>();
        }
    }
}
