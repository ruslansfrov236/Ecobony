
namespace ecobony.application.Feauters.Command
{
    public class CreateBalanceCommandHandler(IBalanceService balanceService) : IRequestHandler<CreateBalanceCommandRequest, CreateBalanceCommandResponse>

    {
        public async Task<CreateBalanceCommandResponse> Handle(CreateBalanceCommandRequest request, CancellationToken cancellationToken)
        {
            await balanceService.Create(request.bonusId, request.bonus);
            return new CreateBalanceCommandResponse();
        }
    }
}