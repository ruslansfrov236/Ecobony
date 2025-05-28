namespace ecobony.application.Feauters.Query.Language;

public class GetByIdCommandHandler(ILanguageService _languageService):IRequestHandler<GetByIdCommandRequest, GetByIdCommandResponse>
{
    public async Task<GetByIdCommandResponse> Handle(GetByIdCommandRequest request, CancellationToken cancellationToken)
    {
      var language=  await _languageService.GetById(request.Id);
        return new GetByIdCommandResponse()
        {
            Language = language
        };
    }
}