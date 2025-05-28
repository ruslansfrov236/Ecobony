namespace ecobony.application.Feauters.Command;

public class UpdateCategoryCommandHandler(ICategoryService _categoryService):IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
{
    public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
      await   _categoryService.Update(new UpdateCategoryWithTranslationDto()
        {
            UpdateCategoryDto = new UpdateCategoryDto_s()
            {
                Id = request.CategoryId.ToString(),
                Pointy = request.Pointy,
                FormFile = request.FormFile
            },
            UpdateCategoryTranslationDto = new UpdateCategoryTranslationDto_s()
            {
                Id = request.Id,
                CategoryId = request.CategoryId,
                Description = request.Description,
                Name = request.Name,
                LanguageId = request.LanguageId
            }
        });
        return new UpdateCategoryCommandResponse();
    }
}