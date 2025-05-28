namespace ecobony.application.Feauters.Query;

public class GetAdminWasteAllCommandHandler(IWasteService _wasteService):IRequestHandler<GetAdminWasteAllCommandRequest, GetAdminWasteAllCommandResponse>
{
    public async Task<GetAdminWasteAllCommandResponse> Handle(GetAdminWasteAllCommandRequest request, CancellationToken cancellationToken)
    {
     var waste =   await _wasteService.GetAdminAll();

        return new GetAdminWasteAllCommandResponse()
        {
            Waste = waste
        };
    }
}