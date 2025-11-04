

using ULearn.Domain.Shared;

namespace ULearn.Infrastructure.Settings;

public class JwtSettings
{
    public string? Key { get; set; } = EnvironmentValues.JWT_KEY;
    public string? Issuer { get; set; } = "ULearn";
    public string? Audience { get; set; } = "ULearnClient";
    public int AccessTokenExpiryMinutes { get; set; } = 1;
}
