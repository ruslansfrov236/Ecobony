using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Command
{
    public class SoftBalanceCommandHandler(IBalanceService balanceService):IRequestHandler<SoftBalanceCommandRequest, SoftBalanceCommandResponse>
    {
        public async Task<SoftBalanceCommandResponse> Handle(SoftBalanceCommandRequest request, CancellationToken cancellationToken)
        {
           await balanceService.SoftDelete(request.Id);
            return new SoftBalanceCommandResponse();    
        }
    }
   
}