namespace ecobony.application.Feauters.Command;

public class DeleteWasteCommandRequest:IRequest<DeleteWasteCommandResponse>
{
    public string Id { get; set; }
}