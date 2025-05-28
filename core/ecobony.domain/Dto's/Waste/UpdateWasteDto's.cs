namespace ecobony.domain.Dto_s;

public class UpdateWasteDto_s
{
    public string Id { get; set; }
    public string Title { get; set; }
    public decimal Weight { get; set; }
    public string Station { get; set; }
    public List<BonusComunity> BonusComunities { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public decimal Score { get; set; }
    public List<WasteProcess> WasteProcesses { get; set; } 
}