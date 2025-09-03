using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E = ecobony.domain.Entities;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetPagedQueryResponse
    {
        public PagedResult<E::UserHistory> PagedResult { get; set; }
    }
}
