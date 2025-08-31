

using ULearn.Application.DependencyInjection;
using ULearn.Api.Middlewares;
using ULearn.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using ULearn.Infrastructure.DependencyInjection;

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

var app = builder.Build();
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();
// app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();