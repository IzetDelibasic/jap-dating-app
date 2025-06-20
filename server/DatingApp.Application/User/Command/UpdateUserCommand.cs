using MediatR;
using DatingApp.Application.Contracts.Requests;

public class UpdateUserCommand : IRequest<bool>
{
    public string Username { get; set; } = string.Empty;
    public UpdateMemberRequest Request { get; set; } = null!;
}