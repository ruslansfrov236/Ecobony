
using ecobony.webapi.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddPersistenceRegistration();
builder.Services.AddApplicationRegistration();
builder.Services.AddInfrastructureRegistration();
builder.Services.AddSignairRegistration();
builder.Services.AddMemoryCache();

var supportedCultures = new[] { "az-AZ", "en-US", "ru-RU" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("az-AZ"), // Default culture
    SupportedCultures = supportedCultures.Select(x => new CultureInfo(x)).ToList(),
    SupportedUICultures = supportedCultures.Select(x => new CultureInfo(x)).ToList()
};

builder.Services.AddControllers(options => { options.Filters.Add<ValidationFilter>(); })
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
    .AddJsonOptions(options =>
    {
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

var app = builder.Build();

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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHubs();
app.Run();