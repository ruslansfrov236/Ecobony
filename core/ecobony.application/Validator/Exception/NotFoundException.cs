namespace ecobony.application.Validator
{
 
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Resource not found.") // Default message
        {
        }

        public NotFoundException(string message) : base(message) // Custom message
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) // Inner exception support
        {
        }
    }
}