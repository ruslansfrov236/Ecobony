using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Command.UserHistory
{
    public class RestoreUserHistoryCommandHandler (IUserHistoryService userHistoryService): IRequestHandler<RestoreUserHistoryCommandRequest, RestoreUserHistoryCommandResponse>
    {
        public async Task<RestoreUserHistoryCommandResponse> Handle(RestoreUserHistoryCommandRequest request, CancellationToken cancellationToken)
        {
            await userHistoryService.RestoreDelete(request.Id);
            return new RestoreUserHistoryCommandResponse();
        }
    }
}
