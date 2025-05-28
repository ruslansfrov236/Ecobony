namespace ecobony.domain.Entities;

public class Bonus:BaseEntity
{
    public string? UserId { get; set; }
    public AppUser? User { get; set; }

    public decimal Point { get; set; }
   
    public List<BonusComunity>? BonusComunities { get; set; }
    
    
}