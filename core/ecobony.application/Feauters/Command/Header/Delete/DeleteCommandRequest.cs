namespace ecobony.application.Feauters.Command.Header;

public class DeleteCommandRequest:IRequest<DeleteCommandResponse>
{
    public string Id { get; set; }
}