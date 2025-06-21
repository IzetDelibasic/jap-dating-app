namespace DatingApp.Exceptions;

public class InternalServerException : BaseException
{
    public InternalServerException(string message, string? details = null)
        : base(500, message, details)
    {
    }
}