namespace ecobony.application.Feauters.Query;

public class GetClientCategoryAllCommandHandler(ICategoryService _categoryService):IRequestHandler<GetClientCategoryAllCommandRequest, GetClientCategoryAllCommandResponse>
{
    public async Task<GetClientCategoryAllCommandResponse> Handle(GetClientCategoryAllCommandRequest request, CancellationToken cancellationToken)
    {
     var category =   await _categoryService.GetClientAll();
        return new GetClientCategoryAllCommandResponse()
        {
            Category = category
        };
    }
}