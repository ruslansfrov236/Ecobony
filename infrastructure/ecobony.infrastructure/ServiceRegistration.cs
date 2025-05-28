
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
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = configuration["Token:Audience"],
                    ValidIssuer = configuration["Token:Issuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])),
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                        expires != null && expires > DateTime.UtcNow,
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }
}