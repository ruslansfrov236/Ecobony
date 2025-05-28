using Microsoft.Extensions.DependencyInjection;

namespace ecobony.signair;

public static class ServiceRegistration
{
    public static void AddSignairRegistration(this IServiceCollection collection)
    {
        collection.AddSignalR();
        collection.AddScoped<AuthHubService>();
    }
}