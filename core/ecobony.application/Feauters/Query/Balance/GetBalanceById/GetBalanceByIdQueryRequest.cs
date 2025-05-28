using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query
{
    public class GetBalanceByIdQueryRequest:IRequest<GetBalanceByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}