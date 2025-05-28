namespace ecobony.application.Feauters.Command;

public class RestoreBonusCommandHandler(IBonusService _bonusService):IRequestHandler<RestoreBonusCommandRequest, RestoreBonusCommandResponse>
{
    public async Task<RestoreBonusCommandResponse> Handle(RestoreBonusCommandRequest request, CancellationToken cancellationToken)
    {
        await _bonusService.RestoreDelete(request.Id);
        return new RestoreBonusCommandResponse();
    }
}