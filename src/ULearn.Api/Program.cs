

using ULearn.Application.DependencyInjection;
using ULearn.Api.Middlewares;
using ULearn.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using ULearn.Infrastructure.DependencyInjection;
using ULearn.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Load env
DotNetEnv.Env.Load();
DotNetEnv.Env.TraversePath().Load();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddInfrastructureServices(builder.Configuration).AddApplicationServices(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddScoped<CustomValidationFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery(options =>
{
    var prefix = builder.Environment.IsDevelopment() ? "__Antiforgery-" : "__Host-Antiforgery";
    options.Cookie.Name = prefix + "Token";

    // Secure policy: relaxed in dev, strict in prod
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;

    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.HeaderName = "X-CSRF-TOKEN";
});
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();
// app.UseCors();
app.UseSwaggerDocumentation();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgeryValidation();
app.UseWebsocketMiddleware();
app.MapControllers();

app.Run();