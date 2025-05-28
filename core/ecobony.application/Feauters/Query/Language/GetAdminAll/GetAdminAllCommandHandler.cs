namespace ecobony.application.Feauters.Query.Language;

public class GetAdminAllCommandHandler(ILanguageService _languageService):IRequestHandler<GetAdminAllCommandRequest, GetAdminAllCommandResponse>
{
    public async Task<GetAdminAllCommandResponse> Handle(GetAdminAllCommandRequest request, CancellationToken cancellationToken)
    {
    var language =    await _languageService.GetAdminAll();

        return new GetAdminAllCommandResponse()
        {
            Language = language
        };
    }
}