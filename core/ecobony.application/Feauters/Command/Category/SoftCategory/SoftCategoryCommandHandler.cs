namespace ecobony.application.Feauters.Command;

public class SoftCategoryCommandHandler(ICategoryService _categoryService):IRequestHandler<SoftCategoryCommandRequest,SoftCategoryCommandResponse>
{
    public async Task<SoftCategoryCommandResponse> Handle(SoftCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        await _categoryService.SoftDelete(request.Id);
        return new SoftCategoryCommandResponse();
    }
}