namespace ecobony.application.Feauters.Query.Header;

public class GetAdminAllCommandHandlerHandler(IHeaderService _headerService):IRequestHandler<GetAdminAllCommandHandlerRequest, GetAdminAllCommandHandlerResponse>
{
    public async Task<GetAdminAllCommandHandlerResponse> Handle(GetAdminAllCommandHandlerRequest request, CancellationToken cancellationToken)
    {
      var header =  await _headerService.GetAdminAll();
        return new GetAdminAllCommandHandlerResponse()
        {
            Header = header
        };
    }
}