namespace ecobony.application.Feauters.Command;

public class SoftBonusCommandHandler(IBonusService _bonusService):IRequestHandler<SoftBonusCommandRequest, SoftBonusCommandResponse>
{
    public async Task<SoftBonusCommandResponse> Handle(SoftBonusCommandRequest request, CancellationToken cancellationToken)
    {
        await _bonusService.SoftDelete(request.Id);
        return new SoftBonusCommandResponse();
    }
}