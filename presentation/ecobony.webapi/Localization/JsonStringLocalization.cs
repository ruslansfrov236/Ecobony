using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace ecobony.webapi.Localization;

public class JsonStringLocalization : IStringLocalizer
{

    private readonly IDistributedCache _cache;
   
    private const string ResourceFilePath = "Resources/i18.json";


    public JsonStringLocalization(IDistributedCache cache   )
    {
        _cache = cache;
       
    }

    private static string CurrentCultureName => CultureInfo.CurrentCulture.TwoLetterISOLanguageName;





    public LocalizedString this[string name]
    {
        get
        {
            string? value = GetStringByName(name);
            return new LocalizedString(name, value ?? name, value is null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            LocalizedString localizedValue = this[name];
            return !localizedValue.ResourceNotFound
                ? new LocalizedString(name, string.Format(localizedValue.Value, arguments), false)
                : localizedValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        using FileStream fileStream = new(ResourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader streamReader = new(fileStream);
        using JsonDocument jsonDocument = JsonDocument.Parse(streamReader.ReadToEnd());

        foreach (JsonElement item in jsonDocument.RootElement.EnumerateArray())
        {
            if (item.TryGetProperty("Key", out JsonElement keyElement) &&
                item.TryGetProperty("LocalizedValue", out JsonElement localizedValues))
            {
                string? value = null;

                if (localizedValues.TryGetProperty(CurrentCultureName, out JsonElement locValue))
                {
                    value = locValue.GetString();
                }
                else
                {
                    var shortCulture = CurrentCultureName.Split('-')[0];
                    foreach (var prop in localizedValues.EnumerateObject())
                    {
                        if (prop.Name.StartsWith(shortCulture, StringComparison.OrdinalIgnoreCase))
                        {
                            value = prop.Value.GetString();
                            break;
                        }
                    }
                }

                yield return new LocalizedString(keyElement.GetString()!, value ?? keyElement.GetString()!, value is null);
            }
        }
    }

    private string? GetStringByName(string name)
    {
        string cacheKey = GenerateCacheKey(name);
        string? cachedValue = _cache.GetString(cacheKey);

        if (!string.IsNullOrEmpty(cachedValue)) return cachedValue;

        string? resourceValue = GetValueFromResourceFile(name);

        if (!string.IsNullOrEmpty(resourceValue))
            _cache.SetString(cacheKey, resourceValue);

        return resourceValue;
    }

    private string? GetValueFromResourceFile(string key)
    {
        using FileStream fileStream = new(ResourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader streamReader = new(fileStream);
        using JsonDocument jsonDocument = JsonDocument.Parse(streamReader.ReadToEnd());

        foreach (JsonElement item in jsonDocument.RootElement.EnumerateArray())
        {
            if (!item.TryGetProperty("Key", out JsonElement keyElement) || keyElement.GetString() != key)
                continue;

            if (item.TryGetProperty("LocalizedValue", out JsonElement localizedValues))
            {
                var culture = CurrentCultureName;

                if (localizedValues.TryGetProperty(culture, out JsonElement localizedValue))
                    return localizedValue.GetString() ?? string.Empty;

                var shortCulture = culture.Split('-')[0];
                foreach (var prop in localizedValues.EnumerateObject())
                {
                    if (prop.Name.StartsWith(shortCulture, StringComparison.OrdinalIgnoreCase))
                        return prop.Value.GetString() ?? string.Empty;
                }
            }
        }

        return null;
    }

    private string GenerateCacheKey(string name)
    {
        return $"locale_{CurrentCultureName}_{name}";
    }
}
