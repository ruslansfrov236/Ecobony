using ecobony.webapi.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json.Serialization;
using FluentValidation;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        builder.Services.AddPersistenceRegistration();
        builder.Services.AddApplicationRegistration();
        builder.Services.AddInfrastructureRegistration();
        builder.Services.AddSignairRegistration(builder.Configuration);
        builder.Services.AddJwtAuthorizeRegistration(builder.Configuration);
        builder.Services.AddMemoryCache();
        // -------------------- Services --------------------
        // Localization
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizationFactory>();
        builder.Services.AddSingleton<IStringLocalizer, JsonStringLocalization>();
        builder.Services.AddHttpContextAccessor();

        // FluentValidation global options
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        // Caching & Session
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromDays(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        // Controllers with FluentValidation
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
            options.Filters.Add<UserTrackingActionFilter>();
            options.Filters.Add<IdempotencyActionFilter>();
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new CultureDateTimeConverter());
            options.JsonSerializerOptions.Converters.Add(new CultureNullableDateTimeConverter());
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .AddViewLocalization()
        .AddDataAnnotationsLocalization();
      
        builder.Services.AddHttpContextAccessor();

        // Supported cultures
        var supportedCultures = new[]
        {
            new CultureInfo("az-AZ"),
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU")
        };

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("az-AZ");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });

        // Swagger & Response Compression
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.SwaggerAuthorizeToken();
        builder.SerilogConfigure();
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/json", "text/html", "text/css", "application/javascript" }
            );
        });
        builder.Services.Configure<BrotliCompressionProviderOptions>(o => o.Level = CompressionLevel.Optimal);
        builder.Services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Optimal);

        var app = builder.Build();

        // -------------------- Middleware Pipeline --------------------
        app.UseResponseCompression();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // 1️⃣ RequestLocalization must come first
        var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(locOptions.Value);

        // 2️⃣ Session must come before custom middleware
        app.UseSession();

        // 3️⃣ Custom middleware
        app.UseMiddleware<LocalizationMiddleware>();
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<RateLimitingMiddleware>();
        app.UseMiddleware<CompressResponseMiddleware>();
       
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHubs();

        // Seed admin user & roles
        //await app.SeedAdminUserAndRolesAsync();

        app.Run();
    }
}
