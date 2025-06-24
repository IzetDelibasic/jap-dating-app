using MediatR;
using DatingApp.Application.Contracts.Responses;

public class GetMessageThreadQuery : IRequest<IEnumerable<MessageResponse>>
{
    public string CurrentUsername { get; set; } = string.Empty;
    public string RecipientUsername { get; set; } = string.Empty;
}