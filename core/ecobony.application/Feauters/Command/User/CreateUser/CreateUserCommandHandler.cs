namespace ecobony.application.Feauters.Command;

public class CreateUserCommandHandler(IUserService _userService):IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        await _userService.CreateAsync(new CreateUserDto_s()
        {
            Email = request.Email,
            Password = request.Password,
            UserName = request.UserName
        });
        return new CreateUserCommandResponse();
    }
}