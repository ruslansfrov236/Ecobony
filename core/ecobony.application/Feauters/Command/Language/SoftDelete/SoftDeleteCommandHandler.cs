namespace ecobony.application.Feauters.Command.Language;

public class SoftDeleteCommandHandler(ILanguageService _languageService):IRequestHandler<SoftDeleteCommandRequest, SoftDeleteCommandResponse>
{
    public async Task<SoftDeleteCommandResponse> Handle(SoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        await _languageService.SoftDelete(request.Id);
        return new SoftDeleteCommandResponse();
    }
}