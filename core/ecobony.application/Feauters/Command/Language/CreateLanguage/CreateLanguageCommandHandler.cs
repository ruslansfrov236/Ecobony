using ecobony.application.Validator;
using FluentValidation;

namespace ecobony.application.Feauters.Command.Language;

public class CreateLanguageCommandHandler(ILanguageService _languageService , IStringLocalizer stringLocalizer):IRequestHandler<CreateLanguageCommandRequest, CreateLanguageCommandResponse>
{
    public async Task<CreateLanguageCommandResponse> Handle(CreateLanguageCommandRequest request, CancellationToken cancellationToken)
    {
      

        await _languageService.Create(new CreateLanguageDto_s()
        {
            IsoCode = request.IsoCode,
            Key = request.Key,
            Name = request.Name,
            Image = request.Image,
            FormFile = request.FormFile
        });
        return new CreateLanguageCommandResponse(
            );
    }
}