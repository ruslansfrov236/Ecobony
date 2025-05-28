using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Command
{
    public class DeleteBalanceCommandRequest:IRequest<DeleteBalanceCommandResponse>
    {
        public string? Id { get; set; }
    }
}