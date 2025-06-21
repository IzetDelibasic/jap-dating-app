using DatingApp.Application.Contracts.Responses;
using DatingApp.Entities;
using DatingApp.Helpers;

namespace DatingApp.Repository.Interfaces;

public interface IUserRepository : IBaseRepository<AppUser>
{
    Task<MemberResponse?> GetMemberAsync(string username, bool isCurrentUser);
    Task<PagedList<MemberResponse>> GetMembersAsync(UserParams userParams);
    Task<AppUser?> GetUserByPhotoId(int photoId);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<AppUser>> GetUsersAsync();
}