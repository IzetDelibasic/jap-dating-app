namespace DatingApp.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message, string? details = null)
        : base(401, message, details)
    {
    }
}