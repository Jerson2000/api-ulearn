

using ULearn.Domain.Enums;

namespace ULearn.Domain.Shared;

public static class EnvironmentValues
{
    public static string? DB_CONNECTION => Environment.GetEnvironmentVariable("DB_CONNECTION");

    #region Redis

    public static bool IS_REDIS_CACHED_ENABLE => bool.TryParse(Environment.GetEnvironmentVariable("IS_REDIS_CACHED_ENABLE"), out var isEnabled) && isEnabled;

    public static string? REDIS_HOST => Environment.GetEnvironmentVariable("REDIS_HOST");
    public static string? REDIS_PASSWORD => Environment.GetEnvironmentVariable("REDIS_PASSWORD");
    public static string? REDIS_PORT => Environment.GetEnvironmentVariable("REDIS_PORT");

    #endregion


    public static string? JWT_KEY => Environment.GetEnvironmentVariable("JWT_KEY");


    #region Email

    public static EmailProviderEnum? EMAIL_PROVIDER => Enum.TryParse<EmailProviderEnum>(Environment.GetEnvironmentVariable("EMAIL_PROVIDER"), true, out var parsedEnum) ? parsedEnum : EmailProviderEnum.SMTP;
    public static string? EMAIL_SERVER => Environment.GetEnvironmentVariable("EMAIL_SERVER");
    public static string? EMAIL_USERNAME => Environment.GetEnvironmentVariable("EMAIL_USERNAME");
    public static string? EMAIL_PASSWORD => Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
    public static int EMAIL_PORT => int.TryParse(Environment.GetEnvironmentVariable("EMAIL_PORT"), out var port) ? port : 587;
    public static string? EMAIL_API_KEY => Environment.GetEnvironmentVariable("EMAIL_API_KEY");

    #endregion

}
