namespace ecobony.application.Feauters.Command;

public class DeleteBonusCommandRequest : IRequest<DeleteBonusCommandResponse>
{
    public string Id { get; set; }
}