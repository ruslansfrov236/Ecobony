namespace ecobony.application.Feauters.Command;

public class RestoreCategoryCommandHandler(ICategoryService _categoryService):IRequestHandler<RestoreCategoryCommandRequest, RestoreCategoryCommandResponse>
{
    public async Task<RestoreCategoryCommandResponse> Handle(RestoreCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        await _categoryService.Restore(request.Id);
        return new RestoreCategoryCommandResponse();
    }
}