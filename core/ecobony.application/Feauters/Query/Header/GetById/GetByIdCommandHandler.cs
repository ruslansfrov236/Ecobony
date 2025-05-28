namespace ecobony.application.Feauters.Query.Header;

public class GetByIdCommandHandler(IHeaderService _headerService):IRequestHandler<GetByIdCommandRequest, GetByIdCommandResponse>
{
    public async Task<GetByIdCommandResponse> Handle(GetByIdCommandRequest request, CancellationToken cancellationToken)
    {
     var header=   await _headerService.GetById(request.Id);
        return new GetByIdCommandResponse()
        {
            Header = header
        };
    }
}