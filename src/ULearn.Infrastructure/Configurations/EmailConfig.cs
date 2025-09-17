

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using ULearn.Infrastructure.Services.Emails;
using ULearn.Infrastructure.Settings;

namespace ULearn.Infrastructure.Configurations;

public static class EmailConfig
{
    public static IServiceCollection AddEmailConfig(this IServiceCollection services)
    {
        services.Configure<EmailSetting>(_ => { });
        var settings = services.BuildServiceProvider()
                                   .GetRequiredService<IOptions<EmailSetting>>()
                                   .Value;

        if (EnvironmentValues.EMAIL_PROVIDER == EmailProviderEnum.SMTP)
        {
            Console.WriteLine($"{settings.Username}\t {settings.Password}");
            services.AddSingleton<IEmailService>(provider =>
            new SmtpEmailService(
                settings.Server,
                settings.Port,
                settings.Username,
                settings.Password));
        }

        return services;
    }
}