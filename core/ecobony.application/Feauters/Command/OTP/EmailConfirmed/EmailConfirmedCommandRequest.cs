namespace ecobony.application.Feauters.Command;

public class EmailConfirmedCommandRequest : IRequest<EmailConfirmedCommandResponse>
{
    public string userId { get; set; }
}