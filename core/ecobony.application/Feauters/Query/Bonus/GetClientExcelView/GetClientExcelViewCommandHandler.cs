namespace ecobony.application.Feauters.Query;

public class GetClientExcelViewCommandHandler(IBonusService _bonusService):IRequestHandler<GetClientExcelViewCommandRequest, GetClientExcelViewCommandResponse>
{
    public async Task<GetClientExcelViewCommandResponse> Handle(GetClientExcelViewCommandRequest request, CancellationToken cancellationToken)
    {
        await _bonusService.GetClientExcelView(request.StartDate, request.EndDate);
        return new GetClientExcelViewCommandResponse();
    }
}