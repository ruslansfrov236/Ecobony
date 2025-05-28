namespace ecobony.application.Feauters.Query.Header;

public class GetClientAllCommandHandler(IHeaderService _headerService):IRequestHandler<GetClientAllCommandRequest, GetClientAllCommandResponse>
{
    public async Task<GetClientAllCommandResponse> Handle(GetClientAllCommandRequest request, CancellationToken cancellationToken)
    {
      var header=  await _headerService.GetClientAll();
        return new GetClientAllCommandResponse()
        {
            Header = header
        };
    }
}