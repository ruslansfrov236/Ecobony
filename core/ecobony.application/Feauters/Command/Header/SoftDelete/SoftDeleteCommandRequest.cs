namespace ecobony.application.Feauters.Command.Header;

public class SoftDeleteCommandRequest:IRequest<SoftDeleteCommandResponse>
{
    public string Id { get; set; }
}