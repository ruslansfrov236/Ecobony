

namespace ecobony.domain.Entities
{
    public class Balance:BaseEntity
    {
        public string? UserId { get; set; } 
        public AppUser? User { get; set; }
        
       
      
        public List<BalanceTransfer>? BalanceTransfer { get; set; }

    
    }
}