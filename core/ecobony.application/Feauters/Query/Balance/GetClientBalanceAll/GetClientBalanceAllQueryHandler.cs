using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query
{
    public class GetClientBalanceAllCommandHandler(IBalanceService balanceService):IRequestHandler<GetClientBalanceAllCommandRequest, GetClientBalanceAllCommandResponse>
    {
        public async Task<GetClientBalanceAllCommandResponse> Handle(GetClientBalanceAllCommandRequest request, CancellationToken cancellationToken)
        {
            var balance = await balanceService.GetClientAll();
            
            return new GetClientBalanceAllCommandResponse
            {
                Balance = balance
            };
        }
    }
    
}