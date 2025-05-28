namespace ecobony.application.Feauters.Command
{
    public class CreateBalanceCommandRequest : IRequest<CreateBalanceCommandResponse>

    {
        public string? bonusId { get; set; } 
        public decimal bonus { get; set; }  
    }
}