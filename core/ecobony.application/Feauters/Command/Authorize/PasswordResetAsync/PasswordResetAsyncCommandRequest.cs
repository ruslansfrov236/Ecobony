namespace ecobony.application.Feauters.Command;

public class PasswordResetAsyncCommandRequest : IRequest<PasswordResetAsyncCommandResponse>
{
    public string Email { get; set; }
}