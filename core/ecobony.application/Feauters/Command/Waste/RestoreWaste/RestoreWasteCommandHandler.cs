namespace ecobony.application.Feauters.Command;

public class RestoreWasteCommandHandler(IWasteService _wasteService):IRequestHandler<SoftWasteCommandRequest, SoftWasteCommandResponse>
{
    public async Task<SoftWasteCommandResponse> Handle(SoftWasteCommandRequest request, CancellationToken cancellationToken)
    {
        await _wasteService.Restore(request.Id);
        return new SoftWasteCommandResponse();
    }
}