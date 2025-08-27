namespace ecobony.domain.Entities.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public string? ProfileImage { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
        public bool IsOnline { get; set; }
        public DateTime EntryTime { get; set; }  
        public DateTime? ExitTime { get; set; }   
        public TimeSpan Duration => ExitTime.HasValue ? ExitTime.Value - EntryTime : TimeSpan.Zero;  
        
       
        public string DurationInReadableFormat 
        {
            get
            {
                var totalDays = (int)Duration.TotalDays;
                var hours = Duration.Hours;
                var minutes = Duration.Minutes;

                return $"{totalDays} gün {hours} saat {minutes} dəqiqə";
            }
        }

    
        public double TotalMinutes => Duration.TotalMinutes;  
        public double TotalHours => Duration.TotalHours;     
        public double TotalDays => Duration.TotalDays;       

        public string? NameSurname { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }

        public List<Waste>? Wastes { get; set; }

        public List<Bonus>? Bonuses { get; set; }
        public List<WasteProcess>? WasteProcesses { get; set; }

        public List<UserHistory>? UserHistory { get; set; }
        public List<UserTracking> UserTrackings { get; set; }   
        
        public bool IsCurrentlyOnline()
        {
           
            return !ExitTime.HasValue || ExitTime > DateTime.Now;
        }
   
   
    }
}