namespace ecobony.application.Feauters.Query;

public class GetAdminBonusAllCommandHandler(IBonusService _bonusService):IRequestHandler<GetAdminBonusAllCommandRequest, GetAdminBonusAllCommandResponse>
{
    public async Task<GetAdminBonusAllCommandResponse> Handle(GetAdminBonusAllCommandRequest request, CancellationToken cancellationToken)
    {
        var bonus = await _bonusService.GetAdminAll();
        return new GetAdminBonusAllCommandResponse()
        {
            Bonus = bonus
        };
    }
}