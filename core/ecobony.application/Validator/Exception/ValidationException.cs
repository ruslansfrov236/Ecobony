namespace ecobony.application.Validator;

public class ValidationException : Exception
{
    public ValidationException() : base("Invalid format.") { }

    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception? innerException) : base(message, innerException) { }
}