using ecobony.application.Services;
using ecobony.domain.Entities;
using ecobony.signair.Hubs;
using Microsoft.Extensions.DependencyInjection;

namespace ecobony.signair;

public static class ServiceRegistration
{
    public static void AddSignairRegistration(this IServiceCollection collection)
    {
        collection.AddSignalR();
        collection.AddScoped<AuthHub>();
        collection.AddTransient<IUserTrackingService , UserTrackingService>();  
        collection.AddTransient<UserTrackingHub>();
    }
}