namespace DatingApp.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message, string? details = null)
        : base(400, message, details)
    {
    }
}