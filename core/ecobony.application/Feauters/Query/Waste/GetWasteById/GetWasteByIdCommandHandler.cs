namespace ecobony.application.Feauters.Query;

public class GetWasteByIdCommandHandler(IWasteService _wasteService):IRequestHandler<GetWasteByIdCommandRequest, GetWasteByIdCommandResponse>
{
    public async Task<GetWasteByIdCommandResponse> Handle(GetWasteByIdCommandRequest request, CancellationToken cancellationToken)
    {
      var waste=  await _wasteService.GetById(request.Id);
        return new GetWasteByIdCommandResponse()
        {
            Waste = waste
        };
    }
}