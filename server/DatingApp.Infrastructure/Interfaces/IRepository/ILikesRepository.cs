using DatingApp.Application.Contracts.Responses;
using DatingApp.Entities;
using DatingApp.Helpers;

namespace DatingApp.Repository.Interfaces;

public interface ILikesRepository : IBaseRepository<UserLike>
{
    Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
    Task<PagedList<MemberResponse>> GetUserLikes(LikesParams likesParams);
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
}