using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;

namespace DatingApp.Repository.Interfaces;

public interface ILikesRepository : IBaseRepository<UserLike>
{
    Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
    Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
}