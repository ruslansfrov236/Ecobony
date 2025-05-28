namespace ecobony.application.Feauters.Command;

public class CreateBonusCommandHandler(IBonusService _bonusService):IRequestHandler<CreateBonusCommandRequest, CreateBonusCommandResponse>
{
    public async Task<CreateBonusCommandResponse> Handle(CreateBonusCommandRequest request, CancellationToken cancellationToken)
    {
        await _bonusService.Create(request.wasteId);
        return new CreateBonusCommandResponse();
    }
}