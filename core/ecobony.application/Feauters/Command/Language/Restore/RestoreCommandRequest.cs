namespace ecobony.application.Feauters.Command.Language;

public class RestoreCommandRequest:IRequest<RestoreCommandResponse>
{
    public string Id { get; set; }
}