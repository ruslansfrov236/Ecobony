namespace ecobony.application.Feauters.Command;

public class LoginAsyncCommandHandler(IAuthService _authService):IRequestHandler<LoginAsyncCommandRequest, LoginAsyncCommandResponse>
{
    public async Task<LoginAsyncCommandResponse> Handle(LoginAsyncCommandRequest request, CancellationToken cancellationToken)
    {
        var token = await _authService.LoginAsync(request.password, request.usernameOrEmail,
            request.accessTokenLifeTime, request.isSave);
        return new LoginAsyncCommandResponse()
        {
            Token = token
        };
    }
}