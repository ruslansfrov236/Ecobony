namespace ecobony.domain.Entities;

public class Location:BaseEntity
{
   
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public List<WasteProcess>? WasteProcesses { get; set; } 
}
