using DatingApp.Entities.DTO;
using DatingApp.Helpers;

namespace DatingApp.Services.Interfaces;

public interface IUsersService
{
    Task<PagedList<MemberDto>> GetUsers(UserParams userParams, string currentUsername);
    Task<MemberDto?> GetUser(string username, string currentUsername);
    Task<bool> UpdateUser(string username, MemberUpdateDto memberUpdateDto);
}