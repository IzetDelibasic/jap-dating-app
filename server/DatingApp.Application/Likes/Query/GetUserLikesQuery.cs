using MediatR;
using DatingApp.Helpers;
using DatingApp.Application.Contracts.Responses;

public class GetUserLikesQuery : IRequest<PagedList<MemberResponse>>
{
    public LikesParams LikesParams { get; set; } = null!;
}