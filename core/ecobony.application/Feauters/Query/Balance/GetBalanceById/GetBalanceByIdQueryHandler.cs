using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query
{
    public class GetBalanceByIdQueryHandler(IBalanceService balanceService) : IRequestHandler<GetBalanceByIdQueryRequest, GetBalanceByIdQueryResponse>
    {
        public async Task<GetBalanceByIdQueryResponse> Handle(GetBalanceByIdQueryRequest request, CancellationToken cancellationToken)
        {
         var balance =   await balanceService.GetById(request.Id);
            return new GetBalanceByIdQueryResponse
            {
                Balance = balance
            };
        }
    }
}