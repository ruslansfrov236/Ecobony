namespace ecobony.domain.Entities;

public class WasteProcess:BaseEntity
{
    
    public Guid UserId { get; set; } 
    public AppUser User { get; set; }
    public Guid WasteId { get; set; } 
    public Waste Waste { get; set; }
    public double Weight { get; set; } 
    public Guid LocationId { get; set; } 
    public Location Location { get; set; }
    public bool IsWasteDelivered { get; set; }
    public bool IsBonusAwarded { get; set; } 
   
}