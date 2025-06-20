using MediatR;
using DatingApp.Helpers;
using DatingApp.Application.Contracts.Responses;

public class GetMessagesForUserQuery : IRequest<PagedList<MessageResponse>>
{
    public MessageParams Params { get; set; } = null!;
}