namespace ecobony.application.Feauters.Query;

public class GetClientBonusAllCommandHandler(IBonusService _bonusService):IRequestHandler<GetClientBonusAllCommandRequest, GetClientBonusAllCommandResponse>
{
    public async Task<GetClientBonusAllCommandResponse> Handle(GetClientBonusAllCommandRequest request, CancellationToken cancellationToken)
    {
     var bonus =   await _bonusService.GetClientAll();
        return new GetClientBonusAllCommandResponse()
        {
            Bonus = bonus
        };
    }
}