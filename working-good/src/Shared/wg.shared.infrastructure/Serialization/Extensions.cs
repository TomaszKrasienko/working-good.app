using Newtonsoft.Json;

namespace wg.shared.infrastructure.Serialization;

public static class Extensions
{
    public static string ToJson<T>(this T data)
        => JsonConvert.SerializeObject(data);

    public static T ToObject<T>(this string json)
        => JsonConvert.DeserializeObject<T>(json);
}