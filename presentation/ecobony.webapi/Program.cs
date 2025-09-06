
using ecobony.webapi.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddPersistenceRegistration();
builder.Services.AddApplicationRegistration();
builder.Services.AddInfrastructureRegistration();
builder.Services.AddSignairRegistration(builder.Configuration);
builder.Services.AddJwtAuthorizeRegistration(builder.Configuration);
builder.Services.AddMemoryCache();

var supportedCultures = new[] { "az-AZ", "en-US", "ru-RU" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("az-AZ"), // Default culture
    SupportedCultures = supportedCultures.Select(x => new CultureInfo(x)).ToList(),
    SupportedUICultures = supportedCultures.Select(x => new CultureInfo(x)).ToList()
};

builder.Services.AddControllers(options => { 
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<UserTrackingActionFilter>();

})
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
    .AddJsonOptions(options =>
    {

        options.JsonSerializerOptions.Converters.Add(new CultureDateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new CultureNullableDateTimeConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }).AddViewLocalization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizationFactory>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/json", "text/html", "text/css", "application/javascript" }
    );
});

builder.Services.Configure<BrotliCompressionProviderOptions>(o =>
{
    o.Level = CompressionLevel.Optimal; 
});
builder.Services.Configure<GzipCompressionProviderOptions>(o =>
{
    o.Level = CompressionLevel.Optimal;
});

// Middleware-i pipeline-ə əlavə et
var app = builder.Build();
app.UseResponseCompression();


// Configure the localization middleware
app.UseRequestLocalization(localizationOptions); // Only one call here

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession(); // Session varsa
app.UseLanguageValidator(app.Services.GetRequiredService<IMemoryCache>(), app.Services.GetRequiredService<IHttpContextAccessor>());

// Middleware setup
app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<CompressResponseMiddleware>();



app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHubs();
app.SeedAdminUserAndRolesAsync();
app.Run();