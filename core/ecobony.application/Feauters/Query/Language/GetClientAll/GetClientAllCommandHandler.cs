namespace ecobony.application.Feauters.Query.Language;

public class GetClientAllCommandHandler(ILanguageService _languageService):IRequestHandler<GetClientAllCommandRequest , GetClientAllCommandResponse>
{
    public async Task<GetClientAllCommandResponse> Handle(GetClientAllCommandRequest request, CancellationToken cancellationToken)
    {
     var language=   await _languageService.GetCLientAll();
        return new GetClientAllCommandResponse()
        {
            Language = language
        };
    }
}