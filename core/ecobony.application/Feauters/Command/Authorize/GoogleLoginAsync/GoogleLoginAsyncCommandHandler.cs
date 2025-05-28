namespace ecobony.application.Feauters.Command;

public class GoogleLoginAsyncCommandHandler(IAuthService _authService):IRequestHandler<GoogleLoginAsyncCommandRequest, GoogleLoginAsyncCommandResponse>
{
    public async Task<GoogleLoginAsyncCommandResponse> Handle(GoogleLoginAsyncCommandRequest request, CancellationToken cancellationToken)
    {
        var token = await _authService.GoogleLoginAsync(request.IdToken, request.accessTokenLifeTime);
        return new GoogleLoginAsyncCommandResponse()
        {
            Token = token
        };
    }
}