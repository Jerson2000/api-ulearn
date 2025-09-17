

using ULearn.Domain.Shared;

namespace ULearn.Infrastructure.Settings;

public class EmailSetting
{
    public string Server { get; set; } = EnvironmentValues.EMAIL_SERVER ?? "";
    public string Username { get; set; } = EnvironmentValues.EMAIL_USERNAME ?? "";
    public string Password { get; set; } = EnvironmentValues.EMAIL_PASSWORD ?? "";
    public int Port { get; set; } = EnvironmentValues.EMAIL_PORT;
    public string? ApiKey { get; set; } = EnvironmentValues.EMAIL_API_KEY ?? "";
}