namespace ecobony.application.Feauters.Command;

public class CreateWasteCommandHandler(IWasteService _wasteService):IRequestHandler<CreateWasteCommandRequest , CreateWasteCommandResponse>
{
    public async Task<CreateWasteCommandResponse> Handle(CreateWasteCommandRequest request, CancellationToken cancellationToken)
    {
        await _wasteService.Create(new CreateWasteDto_s()
        {
            CategoryId = request.CategoryId,
            Weight = request.Weight,
            Title = request.Title,
            Station = request.Station
        });
        return new CreateWasteCommandResponse();
    }
}