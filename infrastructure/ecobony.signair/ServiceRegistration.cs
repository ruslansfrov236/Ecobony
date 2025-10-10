using ecobony.application.Services;
using ecobony.domain.Entities;
using ecobony.signair.Hubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ecobony.signair;

public static class ServiceRegistration
{
    public static void AddSignairRegistration(this IServiceCollection collection , IConfiguration configuration)
    {
        collection.AddSignalR().AddStackExchangeRedis(configuration.GetConnectionString("Reddis")); ;

        collection.AddScoped<AuthHub>();
        collection.AddScoped<UserHistoryHub>();
        collection.AddTransient<IUserTrackingService , UserTrackingService>();  
        collection.AddTransient<UserTrackingHub>();
        collection.AddTransient<LogOptionsService>();
        collection.AddTransient<TrashCanHub>();
        collection.AddTransient<TrashCanSignairService>();
    }
}