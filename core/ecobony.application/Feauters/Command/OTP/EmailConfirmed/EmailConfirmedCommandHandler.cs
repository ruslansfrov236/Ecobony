namespace ecobony.application.Feauters.Command;

public class EmailConfirmedCommandHandler(IOTPService _otpService):IRequestHandler<EmailConfirmedCommandRequest, EmailConfirmedCommandResponse>
{
    public async Task<EmailConfirmedCommandResponse> Handle(EmailConfirmedCommandRequest request, CancellationToken cancellationToken)
    {
        await _otpService.EmailConfiremed(request.userId);
        return new EmailConfirmedCommandResponse();
    }
}