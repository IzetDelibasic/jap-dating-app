namespace DatingApp.Exceptions;

public abstract class BaseException : Exception
{
    public int StatusCode { get; }
    public string? Details { get; }
    protected BaseException(int statusCode, string message, string? details = null)
        : base(message)
    {
        StatusCode = statusCode;
        Details = details;
    }

}