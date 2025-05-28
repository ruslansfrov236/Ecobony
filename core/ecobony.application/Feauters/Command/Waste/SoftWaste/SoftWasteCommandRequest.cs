namespace ecobony.application.Feauters.Command;

public class SoftWasteCommandRequest:IRequest<SoftWasteCommandResponse>
{
    public string Id { get; set; }
}