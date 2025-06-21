using MediatR;
using DatingApp.Application.Contracts.Requests;
using DatingApp.Application.Contracts.Responses;

public class CreateMessageCommand : IRequest<MessageResponse?>
{
    public string SenderUsername { get; set; } = string.Empty;
    public SendMessageRequest Request { get; set; } = null!;
}