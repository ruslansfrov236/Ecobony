using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Command
{
    public class RestoreBalanceCommandHandler(IBalanceService balanceService):IRequestHandler<RestoreBalanceCommandRequest, RestoreBalanceCommandResponse>
    {
        public async Task<RestoreBalanceCommandResponse> Handle(RestoreBalanceCommandRequest request, CancellationToken cancellationToken)
        {
            await balanceService.RestoreDelete(request.Id);
            return new RestoreBalanceCommandResponse();
           
        }
    }
   
}