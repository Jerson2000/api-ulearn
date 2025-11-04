

namespace ULearn.Api.Utilities;

public static class CookieManager
{
    // Default cookie options (can be overridden when setting)
    private static readonly CookieOptions DefaultOptions = new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddDays(7)
    };

    /// <summary>
    /// Set a secure cookie with optional custom options.
    /// </summary>
    public static void SetCookie(HttpResponse response, string name, string value, CookieOptions? options = null)
    {
        var mergedOptions = MergeOptions(options);
        response.Cookies.Append(name, value, mergedOptions);
    }

    /// <summary>
    /// Set a secure cookie with optional custom options. ttl param should be in seconds integer.
    /// </summary>    
    /// <param name="ttl">Seconds eg: 2mins = 2 * 60</param>
    public static void SetCookie(HttpResponse response, string key, string value, int ttl)
    {
        var mergedOptions = MergeOptions(new CookieOptions
        {
            Expires = DateTime.UtcNow.AddSeconds(ttl)
        });
        response.Cookies.Append(key, value, mergedOptions);
    }

    /// <summary>
    /// Get a cookie value by name.
    /// </summary>
    public static string? GetCookie(HttpRequest request, string name)
    {
        return request.Cookies[name];
    }

    /// <summary>
    /// Delete a cookie by name.
    /// </summary>
    public static void DeleteCookie(HttpResponse response, string name)
    {
        response.Cookies.Delete(name);
    }

    public static void AuthTokenSet(HttpResponse response, string access, string refresh)
    {
        SetCookie(response, "Secure__token", access, 1 * 60 * 60);
        SetCookie(response, "Secure__h$3rf3r", refresh, 7 * 24 * 60 * 60);
    }

    /// <summary>
    /// Helper to merge provided options with secure defaults.
    /// </summary>
    private static CookieOptions MergeOptions(CookieOptions? options)
    {
        if (options == null)
            return new CookieOptions
            {
                HttpOnly = DefaultOptions.HttpOnly,
                Secure = DefaultOptions.Secure,
                SameSite = DefaultOptions.SameSite,
                Expires = DefaultOptions.Expires
            };

        // Apply defaults if fields aren’t set
        return new CookieOptions
        {
            HttpOnly = options.HttpOnly,
            Secure = options.Secure,
            SameSite = options.SameSite,
            Expires = options.Expires
        };
    }
}