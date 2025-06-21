namespace DatingApp.Application.Contracts.Requests;

public class SendMessageRequest
{
    public required string RecipientUsername { get; set; }
    public required string Content { get; set; }
}