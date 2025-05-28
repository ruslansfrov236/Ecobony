namespace ecobony.application.Feauters.Query.Language;

public class GetByIsoCodeCommandHandler(ILanguageService _languageService):IRequestHandler<GetByIsoCodeCommandRequest, GetByIsoCodeCommandResponse>
{
    public async Task<GetByIsoCodeCommandResponse> Handle(GetByIsoCodeCommandRequest request, CancellationToken cancellationToken)
    {
          await _languageService.GetByIsoCodeAsync(request.isoCode);
          return new GetByIsoCodeCommandResponse();
    }
}