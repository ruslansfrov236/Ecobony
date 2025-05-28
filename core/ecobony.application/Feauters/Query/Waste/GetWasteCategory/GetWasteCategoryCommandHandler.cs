namespace ecobony.application.Feauters.Query;

public class GetWasteCategoryCommandHandler(IWasteService _wasteService):IRequestHandler<GetWasteCategoryCommandRequest, GetWasteCategoryCommandResponse>
{
    public async Task<GetWasteCategoryCommandResponse> Handle(GetWasteCategoryCommandRequest request, CancellationToken cancellationToken)
    {
       var waste = await _wasteService.GetFilterWasteCategory(request.categoryId);
        return new GetWasteCategoryCommandResponse()
        {
            Waste=waste
        };
    }
}