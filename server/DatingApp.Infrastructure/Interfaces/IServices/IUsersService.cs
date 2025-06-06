using DatingApp.Entities.DTO;
using DatingApp.Helpers;

namespace DatingApp.Services.Interfaces;

public interface IUsersService
{
    Task<PagedList<MemberDto>> GetUsers(UserParams userParams, string currentUsername);
    Task<MemberDto?> GetUser(string username, string currentUsername);
    Task<IEnumerable<object>> GetUsersWithRoles();
    Task<bool> EditRoles(string username, string roles);
    Task<IEnumerable<string>> GetUserRoles(string username);
    Task<bool> UpdateUser(string username, MemberUpdateDto memberUpdateDto);
}