using MediatR;
using DatingApp.Application.Contracts.Responses;

public class AddTagCommand : IRequest<bool>
{
    public TagResponse Tag { get; set; } = null!;
}