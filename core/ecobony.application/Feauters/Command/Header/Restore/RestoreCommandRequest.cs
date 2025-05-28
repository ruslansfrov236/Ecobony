namespace ecobony.application.Feauters.Command.Header;

public class RestoreCommandRequest:IRequest<RestoreCommandResponse>
{
    public string Id { get; set; }
}