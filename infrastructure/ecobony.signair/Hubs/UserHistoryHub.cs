using ecobony.application.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.signair.Hubs
{
    public class UserHistoryHub(UserManager<AppUser> userManager ):Hub
    {
        public async Task UserOnline()
        {
            var userCount = await userManager.Users
                .Where(a => a.IsOnline)
                .CountAsync();

            await Clients.All.SendAsync(ReceiveHubName.UserOnlineCount, userCount);
        }
    }
}
