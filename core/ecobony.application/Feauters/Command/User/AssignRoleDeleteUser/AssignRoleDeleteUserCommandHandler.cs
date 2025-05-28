namespace ecobony.application.Feauters.Command;

public class AssignRoleDeleteUserCommandHandler(IUserService _userService):IRequestHandler<AssignRoleDeleteUserCommandRequest, AssignRoleDeleteUserCommandResponse>
{
    public async Task<AssignRoleDeleteUserCommandResponse> Handle(AssignRoleDeleteUserCommandRequest request, CancellationToken cancellationToken)
    {
        await _userService.AssignRoleDeleteUser(request.userId);
        return new AssignRoleDeleteUserCommandResponse();
    }
}