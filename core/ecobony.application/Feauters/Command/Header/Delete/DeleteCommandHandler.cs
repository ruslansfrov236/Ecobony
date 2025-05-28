namespace ecobony.application.Feauters.Command.Header;

public class DeleteCommandHandler(IHeaderService _headerService):IRequestHandler<DeleteCommandRequest, DeleteCommandResponse>
{
    public async Task<DeleteCommandResponse> Handle(DeleteCommandRequest request, CancellationToken cancellationToken)
    {
        await _headerService.Delete(request.Id);

        return new DeleteCommandResponse();
    }
}