namespace ecobony.domain.Dto_s;

public class UpdateLocationDto_s
{
    public string? Id { get; set; }
  
    public string? Address { get; set; }
    
    public List<WasteProcess>? WasteProcesses { get; set; } 
}