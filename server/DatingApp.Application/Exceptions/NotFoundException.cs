namespace DatingApp.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message, string? details = null)
        : base(404, message, details)
    {
    }
}