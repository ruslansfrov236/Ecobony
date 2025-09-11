namespace ecobony.application.Feauters.Query.Language;

public class GetClientAllCommandHandler(ILanguageService _languageService , IStringLocalizer  stringLocalizer):IRequestHandler<GetClientAllCommandRequest , GetClientAllCommandResponse>
{
    public async Task<GetClientAllCommandResponse> Handle(GetClientAllCommandRequest request, CancellationToken cancellationToken)
    {
     var language=   await _languageService.GetCLientAll();
        var message = stringLocalizer.GetString("Message200");
        return new GetClientAllCommandResponse()
        {
            Language = language,
            Message=message

        };
    }
}