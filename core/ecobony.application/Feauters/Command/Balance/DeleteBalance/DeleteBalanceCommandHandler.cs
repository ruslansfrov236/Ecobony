using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Command
{
    public class DeleteBalanceCommandHandler(IBalanceService balanceService) : IRequestHandler<DeleteBalanceCommandRequest, DeleteBalanceCommandResponse>
    {
        public async Task<DeleteBalanceCommandResponse> Handle(DeleteBalanceCommandRequest request, CancellationToken cancellationToken)
        {
            await balanceService.Delete(request.Id);
            return new DeleteBalanceCommandResponse();
        }
    }
}