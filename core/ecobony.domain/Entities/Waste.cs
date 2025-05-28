namespace ecobony.domain.Entities;

public class Waste:BaseEntity
{
    public string? Title { get; set; }
    
    public decimal Weight { get; set; }
    public string? Station { get; set; }
    public List<BonusComunity>? BonusComunities { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public Guid CategoryId { get; set; }
    public decimal PricePoint { get; set; }
    public decimal Score { get; set; }
    public Category? Category { get; set; }
    public List<WasteProcess>? WasteProcesses { get; set; } 
}