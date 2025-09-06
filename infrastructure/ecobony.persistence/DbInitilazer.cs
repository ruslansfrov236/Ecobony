using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.persistence
{
    public class DbInitilazer
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }
        private static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(RoleModel)).Cast<RoleModel>())
            {
                var roleName = role.ToString();
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var appRole = new AppRole { Name = roleName };
                    var result = await roleManager.CreateAsync(appRole);

                    if (!result.Succeeded)
                    {
                        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                        throw new Exception($"Role creation failed: {errors}");
                    }
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            await CreateUserAsync(userManager, "Admin_Adminli ", "admin_ecobony", "admin@ecobony.com", "Admin123!", RoleModel.Admin);
            await CreateUserAsync(userManager, "Manager_Managerli ", "manager_ecobony", "manager@ecobony.com", "Manager123!", RoleModel.Manager);
        }

        private static async Task CreateUserAsync(UserManager<AppUser> userManager, string fullName, string userName, string email, string password, RoleModel role)
        {
            if (await userManager.FindByNameAsync(userName) == null)
            {
                var user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    NameSurname = fullName,
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"User creation failed: {errors}");
                }

                await userManager.AddToRoleAsync(user, role.ToString());
            }
        }
    }
}

