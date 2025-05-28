

namespace ecobony.signair.Services;

public class AuthHubService(UserManager<AppUser> _userManager , IHttpContextAccessor  _contextAccessor):Hub
{
    public static Dictionary<string, string> Users = new();
    public async Task Connect()
    {
        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            throw new NotFoundException("Username could not be found in context.");

        var  user = await _userManager.FindByNameAsync(username)
                   ?? throw new NotFoundException("User not found.");

        user.IsOnline = true;
        user.EntryTime = DateTime.Now.ToLocalTime();

        await _userManager.UpdateAsync(user);
        Users[username] = Context.ConnectionId;

        await Clients.All.SendCoreAsync(ReceiveHubName.UserConnected, new object[] { user });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            throw new NotFoundException("Username could not be found in context.");

        var  user = await _userManager.FindByNameAsync(username)
                    ?? throw new NotFoundException("User not found.");

        Users.Remove(username);

        user.IsOnline = false;
        user.ExitTime=DateTime.Now.ToLocalTime();
        await _userManager.UpdateAsync(user);
        await Clients.All.SendCoreAsync(ReceiveHubName.UserDisconnected, new object[] { user });

    }
}