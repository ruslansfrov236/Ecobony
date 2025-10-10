
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using ecobony.signair.Services;
using Serilog;
using ecobony.domain.Entities;


namespace ecobony.webapi.Filter
{
    public static class ServiceRegistration
    {
     public static void   SwaggerAuthorizeToken(this IServiceCollection services
         
         )
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n"
                    + "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }



        public static void SerilogConfigure(this WebApplicationBuilder builder)
        {
           
            var serviceProvider = builder.Services.BuildServiceProvider();
            var hubContext = serviceProvider.GetRequiredService<LogOptionsService>();



     
   
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.MSSqlServer(
                    connectionString: Configuration.ConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
                
                )
                .WriteTo.Sink(new SignalRSink(hubContext))  // 🔴 Burada argument verilir
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithThreadId()
                .MinimumLevel.Information()
                .CreateLogger();

            builder.Host.UseSerilog();
        }

    }
}
