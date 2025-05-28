namespace ecobony.application.Feauters.Command;

public class VeriftResetTokenCommandRequest : IRequest<VeriftResetTokenCommandResponse>
{
    public string UserId {get; set;}
    public string ResetToken {get; set;}
}