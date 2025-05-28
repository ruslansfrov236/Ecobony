namespace ecobony.application.Feauters.Command;

public class SoftBonusCommandRequest : IRequest<SoftBonusCommandResponse>
{
    public string Id { get; set; }
}