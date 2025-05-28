namespace ecobony.application.Feauters.Command;

public class VerificationCodeAsyncCommandRequest : IRequest<VerificationCodeAsyncCommandResponse>
{
    public string userId { get; set;  }
    public string code { get; set;  }
}