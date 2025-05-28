namespace ecobony.application.Feauters.Command.Header;

public class SoftDeleteCommandHandler(IHeaderService _headerService):IRequestHandler<SoftDeleteCommandRequest, SoftDeleteCommandResponse>
{
    public async Task<SoftDeleteCommandResponse> Handle(SoftDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        await _headerService.SoftDelete(request.Id);

        return new SoftDeleteCommandResponse();
    }
}