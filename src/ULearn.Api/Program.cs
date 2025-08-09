

using ULearn.Application.DependencyInjection;
using ULearn.Api.Middlewares;
using ULearn.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using ULearn.Infrastructure.DependencyInjection;
using ULearn.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddJWTConfiguration(builder.Configuration);

var app = builder.Build();
app.UseExceptionHandler(_ => { });
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();


app.MapControllers();

app.Run();