namespace ecobony.application.Feauters.Query;

public class GetAdminCategoryAllCommandHandler(ICategoryService _categoryService):IRequestHandler<GetAdminCategoryAllCommandRequest ,GetAdminCategoryAllCommandResponse>
{
    public async Task<GetAdminCategoryAllCommandResponse> Handle(GetAdminCategoryAllCommandRequest request, CancellationToken cancellationToken)
    {
       var category = await _categoryService.GetAdminAll(); 
        return new GetAdminCategoryAllCommandResponse()
        {
            Category = category
        };
    }
}