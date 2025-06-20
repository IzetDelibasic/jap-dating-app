using MediatR;
using DatingApp.Helpers;
using DatingApp.Application.Contracts.Responses;

public class GetUsersQuery : IRequest<PagedList<MemberResponse>>
{
    public UserParams UserParams { get; set; } = null!;
    public string CurrentUsername { get; set; } = string.Empty;
}