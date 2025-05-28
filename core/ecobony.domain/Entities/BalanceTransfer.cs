using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.domain.Entities
{
    public class BalanceTransfer:BaseEntity
    {
        
        public Guid BalanceId   { get; set; }
        public Balance? Balance { get; set; }
        public Valyuta  Valyuta { get; set; }
        public decimal Amount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}