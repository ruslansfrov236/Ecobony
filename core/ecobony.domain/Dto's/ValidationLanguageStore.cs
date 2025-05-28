using System.Collections.Concurrent;

namespace ecobony.domain.Dto_s;

public static class ValidationLanguageStore
{
    public static Dictionary<string, string> Data { get; set; } = new();

    public static string Get(string key)
    {
        if (Data != null && Data.TryGetValue(key, out var value))
            return value;

        return $"[{key}]"; 
    }
}
