namespace ecobony.application.Feauters.Command.Language;

public class SoftDeleteCommandRequest:IRequest<SoftDeleteCommandResponse>
{
    public string Id { get; set; }
}