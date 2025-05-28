namespace ecobony.application.Feauters.Command;

public class FacebookLoginAsyncCommandHandler(IAuthService _authService):IRequestHandler<FacebookLoginAsyncCommandRequest,FacebookLoginAsyncCommandResponse>
{
    public async Task<FacebookLoginAsyncCommandResponse> Handle(FacebookLoginAsyncCommandRequest request, CancellationToken cancellationToken)
    {
        var token = await _authService.FacebookLoginAsync(request.authToken, request.accessTokenLifeTime);
        return new FacebookLoginAsyncCommandResponse()
        {
            Token = token
        };
    }
}