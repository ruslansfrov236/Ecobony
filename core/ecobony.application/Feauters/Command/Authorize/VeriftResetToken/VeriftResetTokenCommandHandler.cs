namespace ecobony.application.Feauters.Command;

public class VeriftResetTokenCommandHandler(IAuthService _authService):IRequestHandler<VeriftResetTokenCommandRequest, VeriftResetTokenCommandResponse>
{
    public async Task<VeriftResetTokenCommandResponse> Handle(VeriftResetTokenCommandRequest request, CancellationToken cancellationToken)
    {
        await _authService.VerifyResetTokenAsync(request.ResetToken, request.UserId);

        return new VeriftResetTokenCommandResponse();
    }
}