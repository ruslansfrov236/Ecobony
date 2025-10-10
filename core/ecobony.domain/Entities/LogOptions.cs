using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.domain.Entities
{
    public class LogOptions:BaseEntity
    {

        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string UserName { get; set; }
        public string RequestPath { get; set; }
    }
}
