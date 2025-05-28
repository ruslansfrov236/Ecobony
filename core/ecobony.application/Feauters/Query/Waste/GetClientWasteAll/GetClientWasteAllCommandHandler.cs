namespace ecobony.application.Feauters.Query;

public class GetClientWasteAllCommandHandler(IWasteService _wasteService):IRequestHandler<GetClientWasteAllCommandRequest, GetClientWasteAllCommandResponse>
{
    public async Task<GetClientWasteAllCommandResponse> Handle(GetClientWasteAllCommandRequest request, CancellationToken cancellationToken)
    {
     var waste=   await _wasteService.GetClientAll();
        return new GetClientWasteAllCommandResponse()
        {
            Waste = waste
        };
    }
}