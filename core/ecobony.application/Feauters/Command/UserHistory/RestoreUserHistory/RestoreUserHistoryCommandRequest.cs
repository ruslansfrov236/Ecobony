using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Command.UserHistory
{
    public class RestoreUserHistoryCommandRequest:IRequest<RestoreUserHistoryCommandResponse>
    {
        public string Id { get; set; }  
    }
}
