namespace ecobony.application.Feauters.Command;

public class DeleteCategoryCommandHandler(ICategoryService _categoryService):IRequestHandler<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>
{
    public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        await _categoryService.Delete(request.Id);
        return new DeleteCategoryCommandResponse();
    }
}