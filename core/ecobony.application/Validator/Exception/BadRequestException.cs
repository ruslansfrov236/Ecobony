namespace ecobony.application.Validator;

public class BadRequestException:Exception
{
    public BadRequestException():base("Bad Request occurred."){}
    
    public BadRequestException(string message):base(message){}
    
    public BadRequestException(string message , Exception? innerException):base(message, innerException){}
}