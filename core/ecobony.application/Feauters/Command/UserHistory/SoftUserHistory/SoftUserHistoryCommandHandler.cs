


namespace ecobony.application.Feauters.Command.UserHistory
{
    public class SoftUserHistoryCommandHandler(IUserHistoryService userHistoryService) : IRequestHandler<SoftUserHistoryCommandRequest, SoftUserHistoryCommandResponse>
    {
        public async Task<SoftUserHistoryCommandResponse> Handle(SoftUserHistoryCommandRequest request, CancellationToken cancellationToken)
        {
           await userHistoryService.SoftDelete(request.Id);

            return new SoftUserHistoryCommandResponse()
            {
                Message= ""
            };
        }
    }
}
