using ecobony.domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ecobony.webapi.Filter
{
    public static class DbInitializerExtensions
    {
        public static async Task SeedAdminUserAndRolesAsync(this WebApplication app)
        {
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                await DbInitializer.SeedAsync(userManager, roleManager);
            }
        }
    }
}
