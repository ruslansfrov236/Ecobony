namespace ecobony.application.Feauters.Command;

public class UpdateWasteCommandHandler(IWasteService _wasteService):IRequestHandler<UpdateWasteCommandRequest, UpdateWasteCommandResponse>
{
    public async Task<UpdateWasteCommandResponse> Handle(UpdateWasteCommandRequest request, CancellationToken cancellationToken)
    {
        await _wasteService.Update(new UpdateWasteDto_s()
        {
            Id = request.Id,
            CategoryId = request.CategoryId,
            Weight = request.Weight,
            Title = request.Title,
            Station = request.Station
        });
        return new UpdateWasteCommandResponse();
    }
}