namespace ecobony.application.Feauters.Command;

public class DeleteWasteCommandHandler(IWasteService _wasteService):IRequestHandler<DeleteWasteCommandRequest, DeleteWasteCommandResponse>
{
    public async Task<DeleteWasteCommandResponse> Handle(DeleteWasteCommandRequest request, CancellationToken cancellationToken)
    {
        await _wasteService.Delete(request.Id);
        return new DeleteWasteCommandResponse();
    }
}