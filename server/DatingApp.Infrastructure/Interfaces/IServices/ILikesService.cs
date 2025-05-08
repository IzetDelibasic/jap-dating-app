using DatingApp.Entities.DTO;
using DatingApp.Helpers;

namespace DatingApp.Services.Interfaces
{
    public interface ILikesService
    {
        Task<bool> ToggleLike(int sourceUserId, int targetUserId);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int userId);
        Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
    }
}