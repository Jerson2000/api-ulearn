

namespace ULearn.Domain.Shared;

public static class EnvironmentValues
{
    public static string? DB_CONNECTION => Environment.GetEnvironmentVariable("DB_CONNECTION");
    public static string? REDIS_HOST => Environment.GetEnvironmentVariable("REDIS_HOST");
    public static string? REDIS_PASSWORD => Environment.GetEnvironmentVariable("REDIS_PASSWORD");
    public static string? JWT_KEY => Environment.GetEnvironmentVariable("JWT_KEY");

}
