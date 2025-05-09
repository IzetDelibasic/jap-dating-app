using DatingApp.Entities.DTO;
using DatingApp.Helpers;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Services.Interfaces;

public interface IUsersService
{
    Task<PagedList<MemberDto>> GetUsers(UserParams userParams, string currentUsername);
    Task<MemberDto?> GetUser(string username, string currentUsername);
    Task<bool> UpdateUser(string username, MemberUpdateDto memberUpdateDto);
    Task<PhotoDto?> AddPhoto(string username, IFormFile file);
    Task<bool> SetMainPhoto(string username, int photoId);
    Task<bool> DeletePhoto(string username, int photoId);
}