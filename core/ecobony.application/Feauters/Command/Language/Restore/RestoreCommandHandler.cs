namespace ecobony.application.Feauters.Command.Language;

public class RestoreCommandHandler(ILanguageService _languageService):IRequestHandler<RestoreCommandRequest, RestoreCommandResponse>
{
    public async Task<RestoreCommandResponse> Handle(RestoreCommandRequest request, CancellationToken cancellationToken)
    {
        await _languageService.Restore(request.Id);
        return new RestoreCommandResponse();
    }
}