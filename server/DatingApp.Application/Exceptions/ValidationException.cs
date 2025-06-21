namespace DatingApp.Exceptions;

public class ValidationException : BaseException
{
    public ValidationException(string message, string? details = null)
        : base(400, message, details)
    {
    }
}