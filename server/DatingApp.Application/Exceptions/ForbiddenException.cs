namespace DatingApp.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException(string message, string? details = null)
        : base(403, message, details)
    {
    }
}