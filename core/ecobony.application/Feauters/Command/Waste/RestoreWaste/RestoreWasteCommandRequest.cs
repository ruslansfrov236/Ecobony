namespace ecobony.application.Feauters.Command;

public class RestoreWasteCommandRequest:IRequest<RestoreWasteCommandResponse>
{
    public string Id { get; set; }
}