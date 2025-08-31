

namespace ULearn.Infrastructure.Settings;

public class RateLimitSettings
{
    public int IPPermitLimit { get; set; } = 100;
    public int IPWindowSeconds { get; set; } = 60;
    public int IPSegments { get; set; } = 4;
    public int IPQueueLimit { get; set; } = 5;

    public int UserTokenLimit { get; set; } = 20;
    public int UserTokensPerPeriod { get; set; } = 5;
    public int UserReplenishSeconds { get; set; } = 10;
    public int UserQueueLimit { get; set; } = 5;

    public int AuthPermitLimit { get; set; } = 5;
    public int AuthWindowSeconds { get; set; } = 60;
    public int AuthQueueLimit { get; set; } = 2;
}
