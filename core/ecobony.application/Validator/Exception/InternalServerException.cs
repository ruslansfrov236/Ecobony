namespace ecobony.application.Validator;

public class InternalServerException:Exception
{


    public InternalServerException() : base("An internal server error occurred."){}
    public InternalServerException(string message) : base(message){}
    public InternalServerException(string message, Exception? exception) : base(message, exception){}

}