namespace ecobony.application.Validator;

public class CustomUnauthorizedException : Exception
{
    public CustomUnauthorizedException() : base("Unauthorized access.") { }

    public CustomUnauthorizedException(string message) : base(message) { }

    public CustomUnauthorizedException(string message, Exception? exception) : base(message, exception) { }
}