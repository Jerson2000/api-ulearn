using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ULearn.Application.Interfaces;
using ULearn.Application.Services;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using ULearn.Infrastructure.Settings;

namespace ULearn.Infrastructure.Configurations;

public static class JwtConfig
{
    public static IServiceCollection AddJWTConfig(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtSettings>(_ => { });
        var settings = services.BuildServiceProvider()
                                   .GetRequiredService<IOptions<JwtSettings>>()
                                   .Value;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = settings.Issuer,
                ValidAudience = settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key!)),
                RequireSignedTokens = true,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new Error(
                        ErrorCodeEnum.Unauthorized,
                        "Unauthorized"
                    ));
                },
                OnForbidden = context =>
                {
                    return context.Response.WriteAsJsonAsync(new Error(ErrorCodeEnum.Forbidden, "Forbidden"));
                },
                OnTokenValidated = ctx =>
                {
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    // Log the exception or error
                    // Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorizationBuilder()
       .AddPolicy("Admin", policy => policy.RequireRole(UserRole.Admin.ToString()))
       .AddPolicy("Instructor", policy => policy.RequireRole(UserRole.Instructor.ToString()))
       .SetFallbackPolicy(new AuthorizationPolicyBuilder()
           .RequireAuthenticatedUser()
           .Build());

        return services;
    }
}