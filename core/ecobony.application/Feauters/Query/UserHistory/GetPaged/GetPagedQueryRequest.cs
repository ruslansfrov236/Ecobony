﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetPagedQueryRequest:IRequest<GetPagedQueryResponse>
    {
      public  int pageNumber { get; set; }
       public  int pageSize { get; set; }
       public string sortBy { get; set; }
       public  bool isDescending { get; set; }  
    }
}
