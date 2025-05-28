namespace ecobony.application.Feauters.Command;

public class PasswordResetAsyncCommandHandler(IAuthService _authService):IRequestHandler<PasswordResetAsyncCommandRequest, PasswordResetAsyncCommandResponse>
{
    public async Task<PasswordResetAsyncCommandResponse> Handle(PasswordResetAsyncCommandRequest request, CancellationToken cancellationToken)
    {
        await _authService.PasswordResetAsync(request.Email);
        return new PasswordResetAsyncCommandResponse();
    }
}