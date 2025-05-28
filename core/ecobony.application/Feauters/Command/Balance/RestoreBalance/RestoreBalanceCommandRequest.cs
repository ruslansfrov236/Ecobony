using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Command
{
    public class RestoreBalanceCommandRequest:IRequest<RestoreBalanceCommandResponse>
    {
        public string Id { get; set; }
    }
   
}