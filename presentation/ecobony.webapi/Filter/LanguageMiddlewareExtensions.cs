using Microsoft.Extensions.Caching.Memory;

public static class LanguageMiddlewareExtensions
{
    public static IApplicationBuilder UseLanguageValidator(this IApplicationBuilder app, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
    {
        return app.Use(async (context, next) =>
        {
            string culture = context.Session.GetString("Language") ?? "az-AZ";
            var cultureInfo = new CultureInfo(culture);
            ValidatorOptions.Global.LanguageManager.Culture = cultureInfo;

            if (memoryCache.TryGetValue($"ValidationMessages_{culture}", out Dictionary<string, string> messages))
            {
                ValidationLanguageStore.Data = messages;
            }

            await next();
        });
    }
}