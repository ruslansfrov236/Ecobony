using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query
{
    public class GetAdminBalanceAllQueryHandler(IBalanceService balanceService):IRequestHandler<GetAdminBalanceAllQueryRequest,GetAdminBalanceAllQueryResponse>
    {
        public async Task<GetAdminBalanceAllQueryResponse> Handle(GetAdminBalanceAllQueryRequest request, CancellationToken cancellationToken)
        {
            var balance = await balanceService.GetAdminAll();
           
           return new GetAdminBalanceAllQueryResponse
            {
                Balance = balance
            };
        }
    }
   
}