namespace ecobony.application.Feauters.Command;

public class VerificationCodeAsyncCommandHandler(IOTPService _otpService):IRequestHandler<VerificationCodeAsyncCommandRequest,VerificationCodeAsyncCommandResponse>
{
    public async Task<VerificationCodeAsyncCommandResponse> Handle(VerificationCodeAsyncCommandRequest request, CancellationToken cancellationToken)
    {
        await _otpService.SaveVerificationCodeAsync(request.userId, request.code);
        return new VerificationCodeAsyncCommandResponse();
    }
}