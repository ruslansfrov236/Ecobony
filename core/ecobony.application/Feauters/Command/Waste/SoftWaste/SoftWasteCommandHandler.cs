namespace ecobony.application.Feauters.Command;

public class SoftWasteCommandHandler(IWasteService _wasteService):IRequestHandler<SoftWasteCommandRequest, SoftWasteCommandResponse>
{
    public async Task<SoftWasteCommandResponse> Handle(SoftWasteCommandRequest request, CancellationToken cancellationToken)
    {
      await _wasteService.SoftDelete(request.Id);
      return new SoftWasteCommandResponse();
    }
}