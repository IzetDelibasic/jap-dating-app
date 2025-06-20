using MediatR;
using DatingApp.Application.Contracts.Responses;

public class GetUserByUsernameQuery : IRequest<MemberResponse?>
{
    public string Username { get; set; } = string.Empty;
    public string CurrentUsername { get; set; } = string.Empty;
}