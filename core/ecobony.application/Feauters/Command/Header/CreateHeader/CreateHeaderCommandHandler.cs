namespace ecobony.application.Feauters.Command.Header;

public class CreateHeaderCommandHandler(IHeaderService _headerService):IRequestHandler<CreateHeaderCommandRequest, CreateHeaderCommandResponse>
{
    public async Task<CreateHeaderCommandResponse> Handle(CreateHeaderCommandRequest request, CancellationToken cancellationToken)
    {

        await _headerService.Create( new CreateHeaderWithTranslationDto_s()
        {
            CreateHeaderDto =  new CreateHeaderDto()
            {
                Role = request.Role,
                Image = request.Image,
                FomFile = request.FomFile
            },
            CreateHeaderTranslationDto = new CreateHeaderTranslationDto_s()
            {
                HeaderId = request.HeaderId,
             
                Title = request.Title,
                Description = request.Description,
                LanguageId = request.LanguageId
            }
        });

        return new();
    }
}