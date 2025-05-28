namespace ecobony.application.Feauters.Command;

public class UpdatePasswordCommandHandler(IUserService _userService):IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
{
    public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        await _userService.UpdatedPassword(request.userId, request.resetToken, request.newPassword);
        return new UpdatePasswordCommandResponse();
    }
}