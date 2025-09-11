namespace ecobony.application.Feauters.Query.Header;

public class GetClientAllCommandHandler(IHeaderService _headerService):IRequestHandler<GetHeaderClientAllCommandRequest, GetHeaderClientAllCommandResponse>
{
    public async Task<GetHeaderClientAllCommandResponse> Handle(GetHeaderClientAllCommandRequest request, CancellationToken cancellationToken)
    {
      var header=  await _headerService.GetClientAll();
        return new GetHeaderClientAllCommandResponse()
        {
            Header = header
        };
    }
}