namespace ecobony.application.Feauters.Query;

public class GetCategoryByIdCommandHandler(ICategoryService _categoryService):IRequestHandler<GetCategoryByIdCommandRequest, GetCategoryByIdCommandResponse>
{
    public async Task<GetCategoryByIdCommandResponse> Handle(GetCategoryByIdCommandRequest request, CancellationToken cancellationToken)
    {
     var category =   await _categoryService.GetById(request.Id);
        return new GetCategoryByIdCommandResponse()
        {
            Category = category
        };
    }
}