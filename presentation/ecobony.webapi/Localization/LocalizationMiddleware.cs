using ecobony.domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using System.Globalization;

namespace ecobony.webapi.Localization;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly HashSet<string> AvailableCultures = GetAllCultureNames();
    private static readonly ConcurrentDictionary<string, CultureInfo> CultureCache = new();

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
       
        // 1. Session-dan oxu
        string? cultureKey = context.Request.Cookies.FirstOrDefault(a=>a.Key== "Language").Value ?? "az-AZ";
        // 2. Session boşdursa, header-dən oxu
       

       

        var culture = new CultureInfo(cultureKey);
      
       
       ;
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;




        await _next(context);
    }

    private static bool DoesCultureExist(string cultureKey)
    {
        return AvailableCultures.Contains(cultureKey);
    }

    private static HashSet<string> GetAllCultureNames()
    {
        return CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Select(culture => culture.Name) // Orijinal format saxlanılır
            .ToHashSet(StringComparer.OrdinalIgnoreCase); // Case-insensitive comparison
    }

    private static string? ParseAcceptLanguageHeader(string? headerValue)
    {
        IEnumerable<string>? items = headerValue?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split(';').First().Trim());

        return items?.FirstOrDefault();
    }
}
