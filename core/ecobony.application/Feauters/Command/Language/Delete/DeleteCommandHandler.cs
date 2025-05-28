namespace ecobony.application.Feauters.Command.Language;

public class DeleteCommandHandler(ILanguageService _languageService):IRequestHandler<DeleteCommandRequest, DeleteCommandResponse>
{
    public async Task<DeleteCommandResponse> Handle(DeleteCommandRequest request, CancellationToken cancellationToken)
    {
        await _languageService.Delete(request.Id);
        return new DeleteCommandResponse();
    }
}