
using ecobony.application;

namespace ecobony.infrastructur;

public static class ServiceRegistration
{
    public static void AddInfrastructureRegistration(this IServiceCollection service)
    {
        var redisConnection = Configuration.ConnectionStringReddis;

        service.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisConnection));

        service.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "SampleInstance";

        });

        service.AddTransient<ITokenService, TokenService>();
        service.AddTransient<IOTPService, OTPService>();
        service.AddTransient<IMailService, MailService>();
        service.AddTransient<IFileService, FileService>();

    }

    public static void AddJwtAuthorizeRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true, // Token müddəti yoxlanacaq
                ClockSkew = TimeSpan.Zero, // Token vaxt fərqini sıfıra endirir
                ValidAudience = configuration["Token:Audience"],
                ValidIssuer = configuration["Token:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
        });

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });
    }
}