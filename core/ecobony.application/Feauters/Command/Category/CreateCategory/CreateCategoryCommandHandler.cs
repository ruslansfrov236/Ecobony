namespace ecobony.application.Feauters.Command;

public class CreateCategoryCommandHandler(ICategoryService _categoryService):IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        await _categoryService.Create(new CreateCategoryWithTranslationDto()
        {
            CreateCategoryDto = new CreateCategoryDto_s()
            {
                Pointy = request.Pointy,
                FormFile = request.FormFile

            },
            CreateCategoryTranslationDto = new CreateCategoryTranslationDto_s()
            {
                Description = request.Description,

                Name = request.Name,
                LanguageId = request.LanguageId
            }
        });
        return new CreateCategoryCommandResponse();
    }
}