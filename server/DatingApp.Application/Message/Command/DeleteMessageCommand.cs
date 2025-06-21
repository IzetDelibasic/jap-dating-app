using MediatR;

public class DeleteMessageCommand : IRequest<bool>
{
    public string Username { get; set; } = string.Empty;
    public int MessageId { get; set; }
}