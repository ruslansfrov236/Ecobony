namespace ecobony.application.Feauters.Query;

public class GetAllUsersAsyncCommandHandler(IUserService _userService):IRequestHandler<GetAllUsersAsyncCommandRequest, GetAllUsersAsyncCommandResponse>
{
    public async Task<GetAllUsersAsyncCommandResponse> Handle(GetAllUsersAsyncCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetAllUsersAsync(request.page, request.size);

        return new GetAllUsersAsyncCommandResponse()
        {
            User = user,
            TotalUser = _userService.TotalUsersCount
        };
    }
}