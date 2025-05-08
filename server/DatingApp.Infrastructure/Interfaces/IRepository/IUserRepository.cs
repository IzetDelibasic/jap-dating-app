using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;

namespace DatingApp.Repository.Interfaces;

public interface IUserRepository : IBaseRepository<AppUser>
{
    Task<MemberDto?> GetMemberAsync(string username, bool isCurrentUser);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<AppUser?> GetUserByPhotoId(int photoId);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<AppUser>> GetUsersAsync();
}