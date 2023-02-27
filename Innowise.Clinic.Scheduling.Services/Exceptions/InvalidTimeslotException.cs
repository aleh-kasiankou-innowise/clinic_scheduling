namespace Innowise.Clinic.Scheduling.Services.Exceptions;

public class InvalidTimeslotException : ApplicationException
{
    public InvalidTimeslotException(string message) : base(message)
    {
        
    }
}