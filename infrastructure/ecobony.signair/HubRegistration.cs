
using ecobony.signair.Hubs;

namespace ecobony.signair;

public static  class HubRegistration
{
    public static void MapHubs( this WebApplication webApplication)
    {
        webApplication.MapHub<AuthHub>("hub/user-auth");
        webApplication.MapHub<UserTrackingHub>("hub/user-tracking");
        webApplication.MapHub<UserHistoryHub>("hub/user-history");

    }
}