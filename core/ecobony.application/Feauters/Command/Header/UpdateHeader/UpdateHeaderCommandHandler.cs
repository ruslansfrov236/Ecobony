namespace ecobony.application.Feauters.Command.Header;

public class UpdateHeaderCommandHandler(IHeaderService _headerService):IRequestHandler<UpdateHeaderCommandRequest, UpdateHeaderCommandResponse>
{
    public async Task<UpdateHeaderCommandResponse> Handle(UpdateHeaderCommandRequest request, CancellationToken cancellationToken)
    {
        await _headerService.Update(new UpdateHeaderWithTranslationDto_s()
        {
            UpdateHeaderDto = new UpdateHeaderDto_s()
            {
                Id = request.HeaderId.ToString(),
                Role = request.Role,
                Image = request.Image,
                FomFile = request.FomFile
            },
            UpdateHeaderTranslationDto = new UpdateHeaderTranslationDto_s()
            {
                Id = request.Id,
                HeaderId = request.HeaderId,
                Title = request.Title,
                Description = request.Description,
                LanguageId = request.LanguageId
            }
        });
        return new UpdateHeaderCommandResponse();
    }
}