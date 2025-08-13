
using Newtonsoft.Json;
using System.Text;

namespace ULearn.Api.Utils;

public static class CacheHelper
{
    public static string SerializeToBase64<T>(T data)
    {
        var json = JsonConvert.SerializeObject(data);
        var bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public static T? DeserializeFromBase64<T>(string base64Data)
    {
        if (string.IsNullOrEmpty(base64Data)) return default;

        var bytes = Convert.FromBase64String(base64Data);
        var json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(json);
    }
}