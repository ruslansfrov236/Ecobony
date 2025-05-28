namespace ecobony.application.Feauters.Command;

public class RestoreBonusCommandRequest : IRequest<RestoreBonusCommandResponse>
{
    public string Id { get; set; }
}