namespace ecobony.application.Feauters.Command;

public class CreateBonusCommandRequest : IRequest<CreateBonusCommandResponse>
{
    public string wasteId { get; set; }
}