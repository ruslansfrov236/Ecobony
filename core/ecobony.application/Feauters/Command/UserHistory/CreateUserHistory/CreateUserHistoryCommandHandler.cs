using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Command.UserHistory
{
    public class CreateUserHistoryCommandHandler(IUserHistoryService _userHistoryService) : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _userHistoryService.Create();
            return new CreateUserCommandResponse();
        }
    }
}
