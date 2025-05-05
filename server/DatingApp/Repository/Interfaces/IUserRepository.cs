using DatingApp.Entities;
using DatingApp.Entities.DTO;
using DatingApp.Helpers;

namespace DatingApp.Repository.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    // 7. Ignore Query filter for the current user (GetMemberAsync) so the current user still sees their unapproved photos
    Task<MemberDto?> GetMemberAsync(string username, bool isCurrentUser);
    // 14. Add the logic in the Admin controller approve photo method to check to see if the user has anyphotos that are set to main, if not then set the photo to main when approving.
    Task<AppUser?> GetUserByPhotoId(int photoId);
}
