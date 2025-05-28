
namespace ecobony.signair;

public static  class HubRegistration
{
    public static void MapHubs( this WebApplication webApplication)
    {
        webApplication.MapHub<AuthHubService>("/products-hub");
      
    }
}