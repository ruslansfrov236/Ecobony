namespace ecobony.application.Feauters.Command.Language;

public class DeleteCommandRequest:IRequest<DeleteCommandResponse>
{
    public string Id { get; set; }
    
    
}