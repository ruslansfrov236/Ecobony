namespace ecobony.application.Feauters.Query;

public class GetWasteTrashCommandHandler(IWasteService _wasteService):IRequestHandler<GetWasteTrashCommandRequest, GetWasteTrashCommandResponse>
{
    public async Task<GetWasteTrashCommandResponse> Handle(GetWasteTrashCommandRequest request, CancellationToken cancellationToken)
    {
      var waste =  await _wasteService.GetWasteTrash();
        return new GetWasteTrashCommandResponse()
        {
            Waste = waste
        };
    }
}