namespace ecobony.application.Feauters.Command;

public class AssignRoleToUserAsyncCommandHandler(IUserService _userService):IRequestHandler<AssignRoleToUserAsyncCommandRequest, AssignRoleToUserAsyncCommandResponse>
{
    public async Task<AssignRoleToUserAsyncCommandResponse> Handle(AssignRoleToUserAsyncCommandRequest request, CancellationToken cancellationToken)
    {
        await _userService.AssignRoleToUserAsync(request.userId, request.Roles);
        return new AssignRoleToUserAsyncCommandResponse();
    }
}