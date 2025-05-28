namespace ecobony.application.Feauters.Command.Language;

public class UpdateLanguageCommandHandler(ILanguageService _languageService):IRequestHandler<UpdateLanguageCommandRequest, UpdateLanguageCommandResponse>
{
    public async Task<UpdateLanguageCommandResponse> Handle(UpdateLanguageCommandRequest request, CancellationToken cancellationToken)
    {
        await _languageService.Update(new UpdateLanguageDto_s()
        {
            Id = request.Id,
            IsoCode = request.IsoCode,
            Key = request.Key,
            Name = request.Name,
            Image = request.Image,
            FormFile = request.FormFile
        });

        return new UpdateLanguageCommandResponse();
    }
}