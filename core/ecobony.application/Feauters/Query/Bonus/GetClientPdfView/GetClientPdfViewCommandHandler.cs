namespace ecobony.application.Feauters.Query;

public class GetClientPdfViewCommandHandler(IBonusService _bonusService):IRequestHandler<GetClientPdfViewCommandRequest, GetClientPdfViewCommandResponse>
{
    public async Task<GetClientPdfViewCommandResponse> Handle(GetClientPdfViewCommandRequest request, CancellationToken cancellationToken)
    {
        await _bonusService.GetClinetPdfView(request.StartDate , request.EndDate);
        return new GetClientPdfViewCommandResponse();
    }
}