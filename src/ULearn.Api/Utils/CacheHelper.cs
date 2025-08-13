
using Newtonsoft.Json;
using System.Text;

namespace ULearn.Api.Utils;

public static class CacheHelper
{
    public static byte[] SerializeToBytes<T>(T data)
    {      
        var json = JsonConvert.SerializeObject(data);
        return Encoding.UTF8.GetBytes(json);
    }

    public static T? DeserializeFromBytes<T>(string base64Data)
    {
        var bytes = Convert.FromBase64String(base64Data);
        var json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(json);
    }
}