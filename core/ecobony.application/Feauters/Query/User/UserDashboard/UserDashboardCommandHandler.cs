namespace ecobony.application.Feauters.Query;

public class UserDashboardCommandHandler(IUserService _userService):IRequestHandler<UserDashboardCommandRequest, UserDashboardCommandResponse>
{
    public async Task<UserDashboardCommandResponse> Handle(UserDashboardCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.UserDashboard();

        return new UserDashboardCommandResponse()
        {
            User = user
        };
    }
}